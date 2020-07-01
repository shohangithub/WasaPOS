
    $(function () {
        $('#datetimepicker').datetimepicker({
            format: 'YYYY-MM-DD'
        });


    });
    $(document).ready(function () {
        //Add button click event
        $('#add').click(function () {
            //validation and add order items
            var isAllValid = true;
            if ($('#ProductId').val() == "0") {
                isAllValid = false;
                $('#ProductId').siblings('span.error').css('visibility', 'visible');
            }
            else {
                $('#ProductId').siblings('span.error').css('visibility', 'hidden');
            }


            if (!($('#quantity').val().trim() != '' && (parseInt($('#quantity').val()) || 0))) {
                isAllValid = false;
                $('#quantity').siblings('span.error').css('visibility', 'visible');
            }
            else {
                $('#quantity').siblings('span.error').css('visibility', 'hidden');
            }

            if (!($('#Unit').val().trim() != '' && !isNaN($('#Unit').val().trim()))) {
                isAllValid = false;
                $('#Unit').siblings('span.error').css('visibility', 'visible');
            }
            else {
                $('#Unit').siblings('span.error').css('visibility', 'hidden');
            }

            if (isAllValid) {
                var $newRow = $('#mainrow').clone().removeAttr('id');
                $('.pc', $newRow).val($('#ProductId').val());


                //Replace add button with remove button
                $('#add', $newRow).addClass('remove').val('Remove').removeClass('btn-success').addClass('btn-danger');

                //remove id attribute from new clone row
                $('#ProductId,#quantity,#Unit,#add', $newRow).removeAttr('id');
                $('span.error', $newRow).remove();
                //append clone row
                $('#productiondetailsItems').append($newRow);

                //clear select data
                $('#ProductId').val('0');
                $('#quantity,#Unit').val('');
                $('#orderItemError').empty();
            }

        });

    //remove button click event
    $('#productiondetailsItems').on('click', '.remove', function () {
        $(this).parents('tr').remove();
    });
