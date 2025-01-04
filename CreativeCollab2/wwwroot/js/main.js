$(function () {


    $("#wizard").steps({
        headerTag: "h2",
        bodyTag: "section",
        //transitionEffect: "fade",
        enableAllSteps: true,
        transitionEffectSpeed: 500,
        // labels: {
        //     finish: "Submit",
        //     next: "Forward",
        //     previous: "Backward"
        // }
    });
    $('.wizard > .steps li a').click(function () {
        $(this).parent().addClass('checked');
        $(this).parent().prevAll().addClass('checked');
        $(this).parent().nextAll().removeClass('checked');
    });
    // Custome Jquery Step Button
    $('.forward').click(function () {
        $("#wizard").steps('next');
    })
    $('.backward').click(function () {
        $("#wizard").steps('previous');
    })
    // Select Dropdown
    $('html').click(function () {
        $('.select .dropdown').hide();
    });
    $('.select').click(function (event) {
        event.stopPropagation();
    });
    $('.select .select-control').click(function () {
        $(this).parent().next().toggle();
    })
    $('.select .dropdown li').click(function () {
        $(this).parent().toggle();
        var text = $(this).attr('rel');
        $(this).parent().prev().find('div').text(text);
    })
    $("#wizard-picture").change(function () {
        readURL(this);
    });

    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#wizardPicturePreview').attr('src', e.target.result).fadeIn('slow');
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    $('input:checkbox').change(function () {
        var radioClicked = $(this).attr('id');
        handleClick(radioClicked);
    });

    $(".card").click(function () {
        var checkbox = $(this).find('input[type=checkbox]');
        toggleCheckboxVisibility(checkbox);
        var inputElement = checkbox.attr('id');
        handleClick(inputElement);
        $(this).toggleClass('card-clicked');
    });

    function toggleCheckboxVisibility(checkbox) {
        checkbox.toggleClass('hidden-checkbox');
    }
    function handleClick(element) {
        if ($("#" + element).prop("checked")) {
            // If the element is checked, uncheck it
            $("#" + element).prop("checked", false);
            $("#" + element + "-card").removeClass("active");
        } else {
            // If the element is not checked, check it
            $("#" + element).prop("checked", true);
            $("#" + element + "-card").addClass("active");
        }

    }
    function clickRadio(inputElement) {
        $("#" + inputElement).prop("checked", true);
    }



    function makeActive(element) {
        $("#" + element + "-card").addClass("active");
    }



})

