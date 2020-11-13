// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
/*
 * sumbangan kode dari Youtube : https://www.youtube.com/watch?v=3r6RfShv8m8 31 Okt 2020 04:07
 * Thank a lot.
 */
$(function () {
    //$("#loaderbody").addClass('hide');

    //$(document).bind('ajaxStart', function () {
    //    $("#loaderbody").removeClass('hide');
    //}).bind('ajaxStop', function () {
    //    $("#loaderbody").addClass('hide');
    //});
});

tampilDiPopUp = (url, judul) => {
    $.ajax({
        type: "get",
        url: url,
        success: function (resp) {
            $("#form-modal .modal-body").html(resp);
            $("#form-modal .modal-title").html(judul);
            $("#form-modal").modal('show');

        }
    })
};

jQueryAjaxPostSimple = form => {

    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (resp) {
                $("#tblTampil").html(resp);

                $("#form-modal .modal-body").html('');
                $("#form-modal .modal-title").html('');
                $("#form-modal").modal('hide');
                //$.notify('Pengiriman telah berhasil', {globalPosition: 'top center', className:'success'});
            },
            error: function (err) {
                console.log(err);
            }

        })
    } catch (e) {
        console.log(e);
    }
    return false;
}
jQueryAjaxDeleteSimple = form => {
    if (confirm('Apakah anda yakin untuk menghapus data ini?')) {
        try {
            $.ajax({
                type: "POST",
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (resp) {
                    $("#tblTampil").html(resp);
                    $.notify('Penghapusan telah berhasil', { globalPosition: 'top center', className: 'success' });
                },
                error: function (err) {
                    console.log(err);
                }

            })
        } catch (e) {
            console.log(e);
        }
    }
    return false;
}


jQueryAjaxPost = form => {

    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (resp) {
                if (resp.isValid) {
                    $("#tblTampil").html(resp.html);

                    $("#form-modal .modal-body").html('');
                    $("#form-modal .modal-title").html('');
                    $("#form-modal").modal('hide');
                    //$.notify('Pengiriman telah berhasil', {globalPosition: 'top center', className:'success'});
                } else {
                    $("#form-modal .modal-body").html(resp.html);
                }
            },
            error: function (err) {
                console.log(err);
            }

        })
    } catch (e) {
        console.log(e);
    }
    return false;
}

jQueryAjaxDelete = form => {
    if (confirm('Apakah anda yakin untuk menghapus data ini?')) {
        try {
            $.ajax({
                type: "POST",
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (resp) {
                    $("#tblTampil").html(resp.html);
                    $.notify('Penghapusan telah berhasil', { globalPosition: 'top center', className:'success' });
                },
                error: function (err) {
                    console.log(err);
                }

            })
        } catch (e) {
            console.log(e);
        }
    }
    return false;
}

pilihSumbuDiIsianGrid = select => {
    $.ajax({
        url: "Titik/MaxSumbu",
        type: "get",
        data: { polaId: $("#polaId").val(), eSumId: $("#pilihSumbu").val() },
        success: function (data) {
            $("#sumbuId").val(data);
        },
        error: function () {
            $("#sumbuId").val(1);
        }
    });
}
//-----------------------------------------------------------------
sesuaikanTampilanGaris = (url) => {
    try {
        //$.getJSON(url, new {polaId:polaId,garisId:grsId,skl:skl})

        $.ajax({
            type: "get",
            url: url,
            //data: new{polaId:polId, garisId:grsId, skl:skla},
            contentType: false,
            processData: false,
            success: function (resp) {
                $("#tblTampil").html(resp);
                // rubah tampilan inzet
                tandaiGarisTerpilih(url);
            },
            error: function (err) {
                console.log(err);
            }
        })
    } catch (e) {
        console.log(e);
    }
    return false;
}
tandaiGarisTerpilih = (url) => {
    try {
        var urlIni = url + "&inzet=true";
        $.ajax({
            type: "get",
            url: urlIni,
            contentType: false,
            processData: false,
            success: function (resp) {
                $("#tblInzet").html(resp);
                //alert(urlIni);
            },
            error: function (err) {
                console.log(err);
            }
        })
    } catch (e) {
        console.log(e);
    }
    return false;
}
sesuaikanTampilanKoord = (url,urlInzet) => {
    try {
        $.ajax({
            type: "get",
            url: url,
            //data: new{polaId:polId, garisId:grsId, skl:skla},
            contentType: false,
            processData: false,
            success: function (resp) {
                $("#tblTampil").html(resp);
                // rubah tampilan inzet
                tandaiKoordTerpilih(urlInzet);
            },
            error: function (err) {
                console.log(err);
            }
        })
    } catch (e) {
        console.log(e);
    }
    return false;
}
tandaiKoordTerpilih = (url) => {
    try {
        var urlIni = url + "&inzet=true";
        $.ajax({
            type: "get",
            url: urlIni,
            contentType: false,
            processData: false,
            success: function (resp) {
                $("#tblInzet").html(resp);
                //alert(urlIni);
            },
            error: function (err) {
                console.log(err);
            }
        })
    } catch (e) {
        console.log(e);
    }
    return false;
}
