window.angJsDependencies.push('ui.bootstrap');

app.controller('storeOfflineController', function ($scope, $timeout) {
    $scope.m = {};

    $scope.init = function () {
        var localTime = moment.utc($scope.dateTime, 'YYYY-MM-DD HH:mm').toDate();
        $scope.startDateTime = localTime;
        
        if ($scope.m.Duration !== undefined && $scope.m.Duration > 0)
            $scope.duration = $scope.m.Duration;
        else
            $scope.duration = 30;
        $scope.durationChange();
    };


    $scope.durationChange = function () {
        var dur = $scope.duration;

        if (dur === undefined) {
            $scope.durationTx = "No válido";
            return;
        }

        var txt = dur%60 + ' minutos';
        if (Math.floor(dur / 60) > 0)
            txt = Math.floor(dur / 60) % 24 + ' horas / ' + txt;
        if (Math.floor(dur / 1440) > 0)
            txt = Math.floor(dur / 1440) + ' día(s) / ' + txt;

        $scope.durationTx = txt;
    };

    $scope.validateAndSubmit = function (submit, formId, urlToPost) {
        $scope.utcStartDateTimeTx = moment($scope.startDateTime).utc().format("YYYY-MM-DD HH:mm");
        $timeout(function () {
            submit(formId, urlToPost);
        }, 1);
    };

});