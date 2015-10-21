(function () {
    "use strict";
    angular
        .module(window.constMainApp)
        .controller('uploadFileController', uploadFileController);

    //uploadFileController.$inject = [''];
    function uploadFileController() {
        var vm = this;
        vm.lstErrors = [];
        vm.lstSuccess = [];
        vm.m = {};
    }
})();