$(document).ready(function () {
    $('#rangeDates').datepicker({
        format: 'yyyy/mm/dd',
        startView: 1,
        startDate: '1970/01/01',
        endDate: '2200/01/01'
    });

    $('#monthDate').datepicker({
        format: 'yyyy/mm',
        startView: 1,
        minViewMode: 1,
        maxViewMode: 1
    });

    $('#yearDate').datepicker({
        format: 'yyyy',
        startView: 2,
        minViewMode: 2,
        maxViewMode: 2
    });
});