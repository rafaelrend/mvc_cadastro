//Valida o formulário de Seguro garantia(edição e cadastro)
$(function(){
	$('#formGarantia').validate({
	  	errorElement:'span',
	  	rules:{
			apolice:{
	 			required:true 
			}
		},
	 messages:{
		apolice:{
			required:"Informe o numero da apólice"
		}
		 	
	  }
	});	
});
   