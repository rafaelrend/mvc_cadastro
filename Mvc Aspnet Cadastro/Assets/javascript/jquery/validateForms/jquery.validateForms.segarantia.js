//Valida o formul�rio de Seguro garantia(edi��o e cadastro)
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
			required:"Informe o numero da ap�lice"
		}
		 	
	  }
	});	
});
   