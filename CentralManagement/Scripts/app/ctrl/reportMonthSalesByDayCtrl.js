app.controller('reportController', function ($scope, $http, $timeout) {
    $scope.m = {};


    $scope.search = function (url, data, okHandle) {
        $scope.working = true;
        $scope.okHandle = okHandle;

        $http({
            method: 'POST',
            url: url,
            data: data
        }).success($scope.handleOk)
            .error($scope.handleError);
    };

    $scope.monthByDayVisualize = function (data) {
        var monthSelected = parseInt($scope.m.startDate.substring(0, 2)) - 1;
        var yearTitle = $scope.m.startDate.substring(3, 7);
        var startDate = new Date(yearTitle, monthSelected, 1);
        var monthTitle = startDate.toLocaleString("es-mx", { month: "long" });
        data = $scope.prepareData(data, startDate, monthSelected);
        data.monthSelected = monthSelected;
        data.monthTitle = monthTitle;
        data.yearTitle = yearTitle;
        $scope.drawData(data);
    };

    $scope.drawData = function (data) {
        var canvas;
        data.h = 500;
        data.padding = 50;
        var processedData = data.processedData;

        canvas = d3.select("#canvasReport");
        data.w = canvas.node().getBoundingClientRect().width;

        canvas.selectAll("svg").remove();

        var svg = canvas.append("svg").attr({ "width": data.w, "height": data.h + 100 });
        svg.append("text")
            .attr("x", (data.w / 2))
            .attr("y", 30)
            .attr("text-anchor", "middle")
            .style("font-size", "30px")
            .text("Ventas de " + data.monthTitle + " de " + data.yearTitle);
        var svgDraw = svg.append("g").attr("transform", "translate(0, 50)");

        var axis = setAxis(svgDraw, data);

        var dataRect = svgDraw.selectAll("rect").data(processedData).enter();

        var nSvg = dataRect.append("rect")
            .attr({
                x: function(d, i) { return axis.xScale(i + 1) + 1; },
                y: function(d) { return axis.yScale(d.TotalPerDay); },
                width: data.w / processedData.length - 4,
                height: function(d) { return data.h - axis.yScale(d.TotalPerDay) - data.padding; },
                fill: function(d) { return colorPicker(d.TotalPerDay, data.avgCost); }
            });
        
        var widthBar = ((data.w / data.processedData.length) / 2);

        dataRect.append("text")
          .text(function (d) { return d.TotalPerDay > 0 ? "$" + d.TotalPerDay : ""; })
          .attr({
              "text-anchor": "middle",
              x: function (d, i) { return axis.xScale(i + 1) + widthBar; }, 
              y: function (d) { return axis.yScale(d.TotalPerDay) + 20; }, 
              "font-family": "open sans",
              "font-size": 10,
              "fill": "#fff"
          });

        setTooltips(nSvg);
        $scope.m.hasChart = true;
    };

    function colorPicker(value, threshold) {
        if (value <= threshold) { return "red"; }
        return "#1ab394";
    }

    function setTooltips(svg) {

        d3.selectAll(".tooltip").remove();
        var tooltip = d3.select("body").append("div")
                        .attr("class", "tooltip")
                        .style("opacity", 0);

        svg.on("mouseover", function (d) {
            tooltip.transition()
                .duration(500)
                .style("opacity", .9);
            tooltip.html("Ventas <strong>$" + d.TotalPerDay + "</strong>")
                .style("left", (d3.event.pageX - 15) + "px")
                .style("top", (d3.event.pageY - 28) + "px");
        })
        .on("mouseout", function () {
            tooltip.transition()
                .duration(500)
                .style("opacity", 0);
        });
    }


    function setAxis(svg, data) {
        var xScale = d3.scale.linear().domain([1, data.processedData.length + 1])
                       .range([data.padding, data.w - data.padding]);

        var yScale = d3.scale.linear().domain([0, d3.max(data.processedData, function (d) { return d.TotalPerDay + (d.TotalPerDay * 0.10); })])
                        .range([data.h - data.padding, 0]);

        var xAxisGen = d3.svg.axis().scale(xScale).orient("bottom").ticks(data.processedData.length).tickFormat(function (d) { return (d > data.processedData.length ? "" : d); });
        var yAxisGen = d3.svg.axis().scale(yScale).orient("left").ticks(11).tickFormat(function (d) { return "$ " + d3.format(".")(d); });

        svg.append("g").call(yAxisGen)
            .attr("class", "axis")
            .attr("transform", "translate(" + data.padding + ", 0)");

        var widthBar = ((data.w / data.processedData.length) / 2);

        svg.append("g").call(xAxisGen)
            .attr("class", "axis")
            .attr("transform", "translate(0," + (data.h - data.padding) + ")")
            .selectAll(".tick text")
            .style("text-anchor", "start")
            .attr("x", widthBar - 5);

        svg.selectAll(".axis path")
                    .style("fill", "none")
                    .style("stroke", "#000")
                    .style("shape-rendering", "crispEdges");

        svg.selectAll(".axis line")
                .style("fill", "none")
                .style("stroke", "#000")
                .style("shape-rendering", "crispEdges");

        return { xScale: xScale, yScale: yScale };
    };

    $scope.prepareData = function (data, startDate, monthSelected) {
        var i, dayCount = 0, nextDate = startDate;
        var processedData = [];
        do {
            var sumCost = 0;
            var bFound = false;
            var currentDay = nextDate.getDate();
            for (i = 0; i < data.length; i++) {
                var dayData = data[i];
                if (dayData.Day != currentDay)
                    continue;
                processedData.push(dayData);
                sumCost += dayData.TotalPerDay;
                bFound = true;
                break;
            }

            if (!bFound) {
                processedData.push({
                    Year: nextDate.getFullYear(),
                    Month: nextDate.getMonth() + 1,
                    Day: currentDay,
                    TotalPerDay: 0
                });
            }

            nextDate.setTime(nextDate.getTime() + 86400000);
            dayCount++;
        } while (nextDate.getMonth() == monthSelected);

        return { processedData: processedData, avgCost: (sumCost / i) };
    };

    $scope.export = function (filename) {
        var html = d3.select("svg").attr({ "xmlns": "http://www.w3.org/2000/svg" }).node().outerHTML;

        var svg = new Blob([html], { type: "image/svg+xml;charset=utf-8" }),
            domUrl = self.URL || self.webkitURL || self,
            url = domUrl.createObjectURL(svg);
        //var imgSrc = 'data:image/svg+xml;base64,' + btoa(unescape(encodeURIComponent(html)));

        var canvas = document.querySelector("canvas");
        var context = canvas.getContext("2d");

        var image = new Image;
        image.onload = function () {
            context.drawImage(image, 0, 0);
            var a = document.createElement("a");
            a.download = filename;
            a.href = canvas.toDataURL("image/png");
            a.click();
        };
        image.src = url;
    };

    $scope.handleOk = function (data) {
        $scope.working = false;

        if (data === undefined || data === null) {
            $scope.handleError();
        }
        else if (data.HasError === true) {
            $scope.msgErr = data.Message;
            $scope.hideMsgErr();
        }
        else if (data.HasError === false) {
            $scope.okHandle(data.Data);
        }
    };

    $scope.handleError = function () {
        $scope.working = false;
        $scope.msgErr = "Ocurrió un error de red. Por favor intente más tarde";
        $scope.hideMsgErr();
    };


    $scope.hideMsgErr = function () {
        $timeout(function () {
            $scope.msgErr = "";
        }, 10000);
    };

});