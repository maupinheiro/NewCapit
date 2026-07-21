/* ============================================================
   ERP.JS
   Funções globais do sistema
   WebForms + Bootstrap + DataTables
   ============================================================ */


/* ============================================================
   INICIALIZAÇÃO GERAL
   ============================================================ */


$(document).ready(function () {

    InicializarERP();

});


function InicializarERP() {


    InicializarDataTables();

    InicializarMascaras();

    InicializarSelect2();

}



/* ============================================================
   UPDATEPANEL WEBFORMS
   Reexecuta scripts após PostBack parcial
   ============================================================ */


if (typeof (Sys) !== "undefined") {


    Sys.WebForms.PageRequestManager
        .getInstance()
        .add_endRequest(function () {


            InicializarERP();


        });


}



/* ============================================================
   DATATABLES PADRÃO
   ============================================================ */


function InicializarDataTables() {


    $('.erp-grid').each(function () {


        if ($.fn.DataTable.isDataTable(this)) {


            $(this).DataTable().destroy();


        }


        $(this).DataTable({


            language: {


                url:
                    "//cdn.datatables.net/plug-ins/1.13.6/i18n/pt-BR.json"


            },


            responsive: true,


            pageLength: 25,


            ordering: true,


            searching: true,


            lengthChange: true,


            dom:

                '<"row"' +

                '<"col-md-6"l>' +

                '<"col-md-6"f>' +

                '>'

                +

                'rt'

                +

                '<"row"' +

                '<"col-md-6"i>' +

                '<"col-md-6"p>' +

                '>'


        });



    });



}




/* ============================================================
   MÁSCARAS
   ============================================================ */


function InicializarMascaras() {



    $('.mask-cnpj').mask('00.000.000/0000-00');


    $('.mask-cep').mask('00000-000');


    $('.mask-telefone')
        .mask('(00) 00000-0000');


    $('.mask-celular')
        .mask('(00) 00000-0000');



}






/* ============================================================
   SELECT2
   ============================================================ */


function InicializarSelect2() {


    $('.select2').each(function () {


        if ($(this).hasClass("select2-hidden-accessible")) {


            $(this).select2('destroy');


        }



        $(this).select2({

            width: '100%'

        });



    });


}






/* ============================================================
   PREVIEW DE IMAGEM
   Uso:
   onchange="PreviewImagem(this,'imgLogo')"
   ============================================================ */


function PreviewImagem(input, idImagem) {



    if (input.files && input.files[0]) {



        let reader = new FileReader();



        reader.onload = function (e) {



            $("#" + idImagem)
                .attr("src", e.target.result);



        };



        reader.readAsDataURL(input.files[0]);



    }


}






/* ============================================================
   SWEET ALERT
   ============================================================ */



function MensagemSucesso(texto) {



    Swal.fire({

        icon: 'success',

        title: 'Sucesso',

        text: texto,

        timer: 2000,

        showConfirmButton: false


    });


}




function MensagemErro(texto) {



    Swal.fire({

        icon: 'error',

        title: 'Erro',

        text: texto


    });


}






function ConfirmarExclusao(callback) {



    Swal.fire({


        title: 'Deseja excluir?',

        text: 'Esta operação não poderá ser desfeita.',

        icon: 'warning',

        showCancelButton: true,

        confirmButtonText: 'Sim, excluir',

        cancelButtonText: 'Cancelar'


    }).then((result) => {


        if (result.isConfirmed) {


            callback();


        }


    });



}






/* ============================================================
   BLOQUEIO BOTÃO DUPLO CLIQUE
   ============================================================ */


function BloquearBotao(botao) {


    $(botao)

        .prop("disabled", true)

        .html(
            '<i class="fas fa-spinner fa-spin"></i> Aguarde...'
        );


}







/* ============================================================
   FORMATAÇÃO
   ============================================================ */


function SomenteNumeros(valor) {


    return valor.replace(/\D/g, '');


}




function FormatarCNPJ(valor) {


    valor = SomenteNumeros(valor);


    return valor.replace(

        /^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2}).*/,

        "$1.$2.$3/$4-$5"

    );


}