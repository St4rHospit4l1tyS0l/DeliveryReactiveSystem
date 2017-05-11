
$(function () {
    $("#fileupload").fileupload({
        url: "/api/upload",
        dataType: 'json',
        done: function (e, data) {
            var scope = angular.element($("#uploadFileScopeId")).scope();

            if (data.result.IsSuccess) {
                window.setTimeout(function () {
                    //var api = $('#tree').aciTree('api');
                    //var item = api.selected();
                    //getFiles(api.itemData(item), 0, filter);
                    $('#progress .progress-bar').css('width', '0').html("");
                }, 2000);
                scope.$apply(function () {
                    scope.$emit('fileUploadSuccess', data.result);
                    scope.uf.m.UidFileName = data.result.ResourceName;
                    scope.uf.lstSuccess.push("Se ha adjuntado el archivo de manera correcta");
                });
            } else {
                scope.$apply(function () {
                    scope.uf.lstErrors.push(data.result.Msg);
                });
            }
        },
        fail: function (source, error) {
            var scope = angular.element($("#uploadFileScopeId")).scope();
            scope.$apply(function () {
                scope.uf.m.UidFileName = '';
                scope.uf.lstErrors.push("Se han presentado los siguientes errores:\n\n" + JSON.stringify(error.messages));
            });
            $('#progress .progress-bar').css('width', '0').html("");
        },
        progressall: function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('#progress .progress-bar').css('width', progress + '%').html(progress + "%");
        },
        add: function (e, data) {
            var imageType = $('#imageType').val();
            var params = {};

            if (imageType !== undefined) {
                params.imageType = imageType;
            } 
            
            data.formData = params;
            data.submit();
        }
    });
});

