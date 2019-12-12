

$(document).ready(function () {
    /* init datatables */

    $('#dt-basic-example').dataTable(
        {
            responsive: true,
            columnDefs: [
                {
                    targets: -1,
                    title: '',
                    orderable: false,
                    render: function (data, type, full, meta) {
                        /*
                        -- ES6
                        -- convert using https://babeljs.io online transpiler
                        return `
                        <a href='javascript:void(0);' class='btn btn-sm btn-icon btn-outline-danger rounded-circle mr-1' title='Delete Record'>
                            <i class="fal fa-times"></i>
                        </a>
                        <div class='dropdown d-inline-block dropleft '>
                            <a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                                <i class="fal fa-ellipsis-v"></i>
                            </a>
                            <div class='dropdown-menu'>
                                <a class='dropdown-item' href='javascript:void(0);'>Change Status</a>
                                <a class='dropdown-item' href='javascript:void(0);'>Generate Report</a>
                            </div>
                        </div>`;
                        ES5 example below:
                        */
                        return "\n\t\t\t\t\t\t<a href='javascript:void(0);' class='editrow btn btn-sm btn-icon btn-outline-primary rounded-circle mr-1' title='Ver información'>\n\t\t\t\t\t\t\t<i class=\"fal fa-edit\"></i>\n\t\t\t\t\t\t<a href='javascript:void(0);' class='deleterow btn btn-sm btn-icon btn-outline-primary rounded-circle mr-1' title='Deshabilitar registro'>\n\t\t\t\t\t\t\t<i class=\"fal fa-window-close\"></i>";
                    },
                },
            ]
        });


    var idEdit;

    $('.editrow').click(function () {

        idEdit = $(this).closest('tr').attr('id');
        var value = $(this).closest('tr').data('value');

        $('#editdata').show();
        $('#editinput').val(value);
    });

    $('.deleterow').click(function () {


        var swalWithBootstrapButtons = Swal.mixin(
            {
                customClass:
                {
                    confirmButton: "btn btn-primary",
                    cancelButton: "btn btn-danger mr-2"
                },
                buttonsStyling: false
            });
        swalWithBootstrapButtons
            .fire(
                {
                    title: "Confirmación",
                    text: "Deseas deshabilitar el elemento?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Si",
                    cancelButtonText: "Cancelar",
                    reverseButtons: true
                })
            .then(function (result) {
                if (result.value) {
                    swalWithBootstrapButtons.fire(
                        "Confirmación",
                        "El registro ha sido deshabilitado",
                        "success"
                    );
                }
                else if (
                    // Read more about handling dismissals
                    result.dismiss === Swal.DismissReason.cancel
                ) {
                    swalWithBootstrapButtons.fire(
                        "Cancelación",
                        "Se ha cancelado la acción",
                        "error"
                    );
                }
            });




    });


    $('#canceledit').click(function () {

        $('#editdata').hide();

    });

    $('#saveedit').click(function () {


        var formData = { id: idEdit, Description: $('#editinput').val() };

        $.ajax({
            url: '/Catalog/SaveEditSafeEquipmentAssigned',
            type: "POST",
            data: formData,
            success: function (data, textStatus, jqXHR) {

                Swal.fire(
                    {
                        position: "top-end",
                        type: "success",
                        title: "Actualización exitosa",
                        showConfirmButton: false,
                        timer: 1500
                    }).then((result) => {

                        window.location.href = '/Catalog/EquipoSeguridad';

                    });



            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });


    });


    $('.newrow').click(function () {

        $('#newdata').show();
    });


    $('#cancelnew').click(function () {

        $('#newdata').hide();

    });

    $('#savenew').click(function () {


        var formData = { Description: $('#newinput').val() };

        $.ajax({
            url: '/Catalog/SaveNewSafeEquipmentAssigned',
            type: "POST",
            data: formData,
            success: function (data, textStatus, jqXHR) {

                Swal.fire(
                    {
                        position: "top-end",
                        type: "success",
                        title: "Actualización exitosa",
                        showConfirmButton: false,
                        timer: 1500
                    }).then((result) => {

                        window.location.href = '/Catalog/EquipoSeguridad';

                    });
            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });


    });


});