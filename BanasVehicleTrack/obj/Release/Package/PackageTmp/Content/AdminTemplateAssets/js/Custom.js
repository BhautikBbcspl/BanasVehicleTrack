"use strict";
$(document).ready(function () {
    // Single Search Select
    $(".js-example-basic-single").select2();

    if ($(".js-example-disabled-results").length) {
        $(".js-example-disabled-results").select2();
    }
    // Multi Select
    $(".js-example-basic-multiple").select2();

    // With Placeholder
    $(".js-example-placeholder-multiple").select2({
        placeholder: "Select Your Name"
    });

    //Limited Numbers
    $(".js-example-basic-multiple-limit").select2({
        maximumSelectionLength: 2
    });

    // Tagging Suppoort
    $(".js-example-tags").select2({
        tags: true
    });

    // Automatic tokenization
    $(".js-example-tokenizer").select2({
        tags: true,
        tokenSeparators: [',', ' ']
    });

    // Loading Array Data
    var data = [{
        id: 0,
        text: 'enhancement'
    }, {
        id: 1,
        text: 'bug'
    }, {
        id: 2,
        text: 'duplicate'
    }, {
        id: 3,
        text: 'invalid'
    }, {
        id: 4,
        text: 'wontfix'
    }];

    $(".js-example-data-array").select2({
        data: data
    });

    //RTL Suppoort

    $(".js-example-rtl").select2({
        dir: "rtl"
    });
    // Diacritics support
    $(".js-example-diacritics").select2();

    // Responsive width Search Select
    $(".js-example-responsive").select2();

    $(".js-example-basic-hide-search").select2({
        minimumResultsForSearch: Infinity
    });

    $(".js-example-disabled").select2({
        disabled: true
    });
    $(".js-programmatic-enable").on("click", function () {
        $(".js-example-disabled").prop("disabled", false);
    });
    $(".js-programmatic-disable").on("click", function () {
        $(".js-example-disabled").prop("disabled", true);
    });

    $(".js-example-theme-single").select2({
        theme: "classic"
    });

    function formatRepo(repo) {
        if (repo.loading) return repo.text;

        var markup = "<div class='select2-result-repository clearfix'>" +
            "<div class='select2-result-repository__avatar'><img src='" + repo.owner.avatar_url + "' /></div>" +
            "<div class='select2-result-repository__meta'>" +
            "<div class='select2-result-repository__title'>" + repo.full_name + "</div>";

        if (repo.description) {
            markup += "<div class='select2-result-repository__description'>" + repo.description + "</div>";
        }

        markup += "<div class='select2-result-repository__statistics'>" +
            "<div class='select2-result-repository__forks'><i class='icofont icofont-flash'></i> " + repo.forks_count + " Forks</div>" +
            "<div class='select2-result-repository__stargazers'><i class='icofont icofont-star'></i> " + repo.stargazers_count + " Stars</div>" +
            "<div class='select2-result-repository__watchers'><i class='icofont icofont-eye-alt'></i> " + repo.watchers_count + " Watchers</div>" +
            "</div>" +
            "</div></div>";

        return markup;
    }

    function formatRepoSelection(repo) {
        return repo.full_name || repo.text;
    }
    $(".js-data-example-ajax").select2({
        ajax: {
            url: "https://api.github.com/search/repositories",
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term, // search term
                    page: params.page
                };
            },
            processResults: function (data, params) {
                // parse the results into the format expected by Select2
                // since we are using custom formatting functions we do not need to
                // alter the remote JSON data, except to indicate that infinite
                // scrolling can be used
                params.page = params.page || 1;

                return {
                    results: data.items,
                    pagination: {
                        more: (params.page * 30) < data.total_count
                    }
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) {
            return markup;
        }, // let our custom formatter work
        minimumInputLength: 1,
        templateResult: formatRepo, // omitted for brevity, see the source of this page
        templateSelection: formatRepoSelection // omitted for brevity, see the source of this page
    });


    //custome datable JS
    // Styling js start
    $('#base-style').DataTable({
        /* "dom": '<"top"lfB><"tabs"t><"bottom"ip>',*/
        //'dom': '<"top d-flex"flB>rt<"bottom d-flex"ip>',
        "bJQueryUI": true,

        buttons: [
            {
                extend: 'excelHtml5',
                text: '<span class="fa fa-file-excel-o"></span>',
                footer: true,
                exportOptions: {
                    columns: ':not(:first-child)',
                },
                 customizeData: function (data) {
                     var table = $('#base-style').DataTable();
                    var rows = table.rows({ search: 'applied' }).nodes().to$();

                    for (var i = 0; i < rows.length; i++) {
                        var rowData = data.body[i];
                        var checkboxElement = $(rows[i]).find('input[type="checkbox"]');
                        var toggleValues = [];

                        checkboxElement.each(function () {
                            var isChecked = $(this).prop('checked');
                            toggleValues.push(isChecked.toString());
                        });

                        for (var j = 0; j < toggleValues.length; j++) {
                            var columnIndex = $(checkboxElement[j]).closest('td').index()-1;
                            rowData[columnIndex] = toggleValues[j];
                        }
                    }
                }
            }],

        'columnDefs': [{ 'orderable': false, 'targets': 0 }],
        'aaSorting': [[0]]
    });

    $('#datable-security').DataTable({

        "bJQueryUI": true,

        buttons: [
            {
                extend: 'excelHtml5',
                text: '<span class="fa fa-file-excel-o"></span>',
                footer: true,
                exportOptions: {
                    columns: ':not(:first-child)',
                }
            }],

        'columnDefs': [{ 'orderable': false, 'targets': 0 }],

    });

    $('#datatable-auditor').DataTable({

        "bJQueryUI": true,

        buttons: [
            {
                extend: 'excelHtml5',
                text: '<span class="fa fa-file-excel-o"></span>',
                footer: true,
                exportOptions: {
                    columns: ':not(:first-child)',
                }
            }],

        'columnDefs': [{ 'orderable': false, 'targets': 0 }],

    });

    $('#contractordatatable').DataTable({
        "bJQueryUI": true,

        buttons: [
            {
                extend: 'excelHtml5',
                text: '<span class="fa fa-file-excel-o"></span>',
                footer: true,
                exportOptions: {
                    columns: ':not(:first-child)',
                }
            }],

        'columnDefs': [{ 'orderable': false, 'targets': 0 }],
        'aaSorting': [[0]]
    });

});