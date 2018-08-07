var obj_paginator = {

    botaoPagina: function (cod, prefixo) {
        var f = document.forms[0];

        var selPagina = document.getElementById(prefixo + "selPagina");

        selPagina.selectedIndex = selPagina.selectedIndex + cod;
        f.submit();

    },

    mostrarPagina: function (obj) {

        var f = document.forms[0];
        f.submit();

    }
    ,
    submitPost: function (obj) {
        var f = document.forms[0];
        f.submit();
    }
}
 