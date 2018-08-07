var obj_contato = {


    add_email: function () {

        var div_emails = document.getElementById("div_emails");
        var div_email0 = document.getElementById("div_email0");

        var str = div_email0.innerHTML;

        var qtde_emails = document.getElementById("qtde_emails");

        var indx = parseInt( qtde_emails.value );

        var input_id = "email" + indx.toString();

        str = str.replace("email0", "email" + indx.toString());
        str = str.replace("email0", "email" + indx.toString());
        str = str.replace("glyphicon-plus", "glyphicon-remove-circle");
        str = str.replace("add_email()", "remove_email(" + indx.toString() +") ");

        //document.getElementById("email" + indx.toString()).value = "";


        var newdiv = document.createElement("div");
        newdiv.id = "div_email" + indx.toString();
        newdiv.innerHTML = str;
        div_emails.appendChild(newdiv);



        indx++;
        document.getElementById("qtde_emails").value = indx.toString();

        document.getElementById(input_id).value = "";


    },

    remove_email: function (id) {

        var div_email_filho = document.getElementById("div_email" + id.toString());

        div_email_filho.outerHTML = "";

        delete div_email_filho;

    },

    form_valido: function (form) {

        if (isVazio(form.nome, "Informe o nome!"))
            return false;

        return true;

    }



}