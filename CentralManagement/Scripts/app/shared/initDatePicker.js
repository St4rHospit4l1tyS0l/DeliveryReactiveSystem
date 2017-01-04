$(document).ready(function () {
    $('#rangeDates').datepicker({
        format: 'dd/mm/yyyy',
        startView: 1,
        startDate: '01/01/1970',
        endDate: '01/01/2200'
    });

    $('#monthDate').datepicker({
        format: 'mm/yyyy',
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