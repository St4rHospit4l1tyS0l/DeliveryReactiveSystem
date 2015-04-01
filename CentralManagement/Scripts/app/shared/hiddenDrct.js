app.directive('ngUpdateHidden',function() {
    return function(scope, el, attr) {
        var model = attr['ngModel'];
        scope.$watch(model, function(nv) {
            el.val(nv);
        });
    };
})