//Valida o formul�rio de Eventos(edi��o e cadastro)
$(function(){
	$('#frmCronoEvent').validate({
	  	errorElement:'span',
	  	rules:{
		    nomeEvent:{
	 			required:true 
			}
		},
	 messages:{
		 nomeEvent:{
			required:"Informe o nome do evento"
		}
		 	
	    }
	});	
});
   