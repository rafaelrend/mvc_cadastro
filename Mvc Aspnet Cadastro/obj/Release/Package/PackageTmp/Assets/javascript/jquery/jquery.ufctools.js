
//Re-Escrita as Classes/Closures padrão do JS
 
/**
 * Novo método 'replaceArray' para a classe String
 * Adiciona um array de strings de busca 
 * 
 * @author Michel Felipe <michelphp@gmail.com>
 * @param string|array searchValue Valores a serem localizados na string
 * @param string replaceValue Valor que irá substituir todas as ocorrências de 'searchValue'
 * @requires String.prototype.replace Método original da classe string
 */
String.prototype.replaceArray = function(searchValue, replaceValue){
	var str = this;
	for(var x=0;x<searchValue.length;x++){
		str = str.replace(searchValue[x],replaceValue);
	}
	return str;
	
};

function Configs(confs){
    this.baseUrl = confs.baseUrl;
    this.loadImg = "<span class='loadImg'><img src='"+confs.srcLoadImg+"' alt='carregando...' title='carregando...'/></span>";
    this.editImg = new function(){
    	var img = document.createElement('img');
    	
    	img.setAttribute('class', 'editLine');
    	img.setAttribute('title', 'Editar');
    	img.setAttribute('alt', 'Editar');
    	img.setAttribute('src', confs.srcEditImg);
    	
    	return img;
    };
    this.jQueryParams =  new function(){
    	var arrMonths = ['Janeiro','Fevereiro','Marco','Abril','Maio','Junho','Julho','Agosto','Setembro','Outubro','Novemb.','Dezembro'];
    	var arrDays = ['Dom','Seg','Ter','Qua','Qui','Sex','Sab'];
    	
    	return {datePicker:{dateFormat:'dd/mm/yy',
			 	showOn:'button',
			 	buttonImage:confs.baseUrl+'/public/images/calendar.png',
			 	buttonImageOnly:true,
			 	changeYear:true,
			 	monthNames:arrMonths,
			 	dayNamesMin:arrDays
    		   }};
    };
    this.tmpJson = [];
	
};
/**
 * Objeto global de ações para o widget 'Dialog' do jquery UI
 */
Dialog = {
	loadContent:function(settings){
		$.post(configs.baseUrl+settings.pageRequest,
			   {requestDialog:true},
			   function(data){
				   var rsJson = JSON.parse(data);
				   if(rsJson.result > 0){
					   
					   settings.dialogMsg =  settings.dialogMsg.replace("replaceContent",'<span style="color:#8B0000">'+rsJson.result+'</span>');
					   $(settings.dialogElement).append(settings.dialogMsg);
					   
					   $(settings.dialogElement).dialog({
  							show: "blind",
							hide: "explode"
  						});
				   }
			   }
		);
	}
};

Form = {
	get: function(elementParent){
		if(elementParent.tagName.toLowerCase() == 'form'){
			if(arguments[1]){
				arguments[1].pageRequest = elementParent.action;
			}
			inputs = elementParent.elements;
		}else{
			inputs = $('#'+elementParent.id + ' input');
		}
		
		return inputs;
	},
	actionServer:'insert'
};

$.fn.extend({
	getHtmlForm:function(arrEvent,settings){
		var fnLoad = function(){
			$(settings.parentForm).prepend($(settings.imgLoad));
			
			var valueTypeForm = {};
			('typeForm' in arguments[0] && typeof arguments[0].typeForm != 'object')?valueTypeForm = arguments[0]:valueTypeForm = {typeForm:$(this).val()};
			if(settings.actionToForm == 'edit'){
				for(var i in settings.paramsToForm){
					if(i != 'typeForm'){
						valueTypeForm[i] = settings.paramsToForm[i];
					}
				}
			}
			
			valueTypeForm = JSON.stringify(valueTypeForm);
			$(settings.parentForm).load(configs.baseUrl+settings.pageRequest,
									    {paramsToForm:valueTypeForm},
									    function(){
									    	$.post(configs.baseUrl+settings.pageRequestJson,
										    		  {dataSendJson:'json'},
										    		  function(data){
										    			  var objJson = JSON.parse(data);
										    			  for(var x in settings.elementsjQueryDates){
										    				  switch (settings.elementsjQueryDates[x]) {
																case 'datepicker':
																	$('#'+x).datepicker(objJson.jQueryParams);
																break;
																default:
																	null;
																break;
															 }
										    			  }
										    			  if('callbackForm' in settings){
										    				  settings.callbackForm();
										    			  }
										    			  
										    		  }
									    		  );
									    	}
									   );
		};
		
		for(var y in arrEvent){
			if(arrEvent[y] == 'load'){
				fnLoad(settings.paramsToForm);
			}else{
				return this.bind(arrEvent[y],fnLoad);
			}
		}
	},
	saveForm:function(settings){	
		var inputs = Form.get(this.get(0),settings);
		
	    $(settings.btnSave).click(function(e){
	    	_data = [];
			var line = new Object();
	    	
	    	if(e.target.type == 'submit'){
	    		$(e.target.form).submit(function(){
	    			return false;
	    		});
	    		e.target.type = 'button';
	    	}

			for(var i=0;i<inputs.length;i++){

	            var control = '';

	            if(inputs[i].type == 'select-one'){
	                control = inputs[i].options[inputs[i].selectedIndex].text;
	            }else{
	                control = inputs[i].value;
	                if(!inputs[i].disabled || inputs[i].name in settings.disabledExceptions){
		                if(inputs[i].name){
		                	inputs[i].name = inputs[i].name.replaceArray([/[a-zA-z0-9]{1,}\[/,/\]/],'');

	                		line[inputs[i].name] = control;
		                	
		                }
	                }
	            }

			}
	
			for(var x in settings.moreValues){
				line[x] = settings.moreValues[x];
			}
			line['actionServer'] = Form.actionServer;
		    _data[_data.length] = line;
	    	
	    	//Envia os dados para o servidor
	    	$.sendData({msgConfirm:'Deseja salvar?',
	    			    pageRequest:settings.pageRequest,
	    			    btnSave:e.target,
	    			    loadImg:settings.loadImg,
	    			    onComplete:('onComplete' in settings)?settings.onComplete:null
	    			    },_data);
	    	
	    });
	},
	populateForm: function(settings){
		inputs = Form.get(this.get(0));
		
		$(settings.btnEdit).bind(settings.eventTrigger,function(){
			
			for ( var x in settings.json){
				for(var z=0;z<inputs.length;z++){
					
					for(var y in settings.json[x]){
						inputs[z].name = inputs[z].name.replaceArray([/[a-zA-z0-9]{1,}\[/,/\]/],'');
						
	                	if(settings.colFilter in settings.json[x]){
							if(inputs[z].name == y){
								if($(this).attr('value') == settings.json[x][settings.colFilter]){
								
									inputs[z].value = settings.json[x][y];
								}
		
								if('callbackInputs' in settings){
									inputs = settings.callbackInputs(inputs);
								}
							}	
						
	                	}
	                	
	                	for(var v in settings.inputsDisabled){
	                		if(settings.inputsDisabled[v] == inputs[z].name){
	                			$('#'+inputs[z].id).attr('disabled','disabled')
	                						   	   .attr('class','disabled');
	                		}
	                	}
					
					}
				}
			}
			if('onComplete' in settings){
				settings.onComplete(Form);
			}
			
			//Cancela a ação padrão da tag <a> caso o elemento que executou o evento seja um link
			if($(this).get(0).tagName.toLowerCase() == 'a'){
				return false;
			}
		});
	},
	eraseForm:function(settings){
		inputs = Form.get(this.get(0));
		var erase = function(){
			for(var x=0; x<inputs.length;x++){
				if(inputs[x].value != ''){
					inputs[x].name = inputs[x].name.replaceArray([/[a-zA-z0-9]{1,}\[/,/\]/],'');
					var fieldsErase = inputs[x].name in settings.notErase;
					
					if(fieldsErase === false){
						if(inputs[x].getAttribute('alt') == 'decimal'){
							inputs[x].value = '0,00';
						}else{
							if(inputs[x].disabled === true){
								inputs[x].disabled = false;
								inputs[x].className = '';
							}
							inputs[x].value = '';
						}
					}
				}
				
			}
		};
		
		if(settings.actionTrigger =='load'){
			erase();
		}else{	
			
			$(settings.btnErase).bind(settings.actionTrigger,function(){
				erase();
				$(this).hide();
				Form.actionServer = 'insert';
			});
		}
	},
	deleteForm:function(settings){
		var inputs = Form.get(this.get(0),settings);
		$(settings.btnRemove).click(function(e){
			_data = [];
			var line = new Object();
			Form.actionServer = 'delete';
			
			for(var x=0; x<inputs.length;x++){	
				if(inputs[x].name in settings.elementsCond){
					line[inputs[x].name] = inputs[x].value; 
				}
				for(var y in settings.moreValues){
					line[y] = settings.moreValues[y];
				}
			}
			_data[_data.length] = line;
			_data[0]['actionServer'] = Form.actionServer;
			
			$.sendData({msgConfirm:'Deseja remover?',
						pageRequest:settings.pageRequest,
					    btnSave:e.target,
					    loadImg:settings.loadImg,
					    inputsRequest:inputs,
					    onComplete:('onComplete' in settings)?settings.onComplete:null
					    },_data);
		});
	}
});

$.extend({
		enableElement:function(settings,strType){
	
		$(settings.btnAction).click(function(){
			switch (strType) {
				case 'display':
					$(settings.objDisplay).css('display','block');
					break;
				case 'enable':
					with($(settings.objDisplay)){
						if(css('display') == 'none'){
							show();
							removeAttr('disabled');
							if(settings.disabled === true){
								attr('class',$(settings.objDisplay).attr('class')+' disabled');
								focus(function(){
									blur();
								});
							}
							('setValue'in settings)?val(settings.setValue):null;
						}else{
							attr('disabled','disabled');
							hide();
						}
					}
				break;
				default:
					null;
				break;
			}
		});
	},
	
	sendData :function(objSend,data){
        var strData = JSON.stringify(data);
        if(strData.length <=3){
                alert('Por favor, preencha todos os campos!!');
                return false;
        }

        var conf = confirm(objSend.msgConfirm);
        if(conf){
            $('span:last').remove();

            $(objSend.loadImg).insertAfter($(objSend.btnSave));

            $.post(configs.baseUrl+objSend.pageRequest,
                  {dataJson:strData},
                     function(val){
                        var rsJson = JSON.parse(val);
                            $('.loadImg').remove();
                            var regexError = /^Erro:/;
						 	if(!regexError.test(rsJson.msg)){
						 		('onComplete' in objSend && typeof objSend.onComplete == 'function')?objSend.onComplete(rsJson,('inputsRequest' in objSend)?objSend.inputsRequest:null):null;
						 	}
                            alert(rsJson.msg);
                            
                        }
            );
        }else{
            return false;
        }

	},

	
	/**
	 * Adiciona options a um elemento select, com dados de um json
	 * 
	 * @author Michel Felipe <michelphp@gmail.com>
	 * @param string elementSelect String do id ou classe do elemento select que terá os options adicionados
	 * @param object jsonValues Json(array de objetos) contendo os dados a serem adicionados aos values dos options
	 * @return void
	 * @public
	 */
	createOptions: function(elementSelect,jsonValues,returnLastValue){
		if(elementSelect.length > 0){
			$(elementSelect + ' option').remove();
		}

                if(typeof jsonValues == 'object'){
                    for ( var x in jsonValues) {
                            for(var y in jsonValues[x]){
                                    var objOpt = document.createElement('option');
                                    objOpt.value = jsonValues[x][y];
                                    objOpt.innerHTML = jsonValues[x][y];
                                    $(elementSelect).append($(objOpt));


                            }
                    }
                }
                if(returnLastValue ){
                    var lastOpt =  $(elementSelect+' option:last').get(0);
                    if(typeof lastOpt != 'undefined'){
                        return lastOpt.text;
                    }else{
                        return '';
                    }
                }
		
	},
	
	/**
	 * ListBox - Adiciona elementos a um select, criando uma listagem a ser salva no db com uma requisição ajax
	 * 
	 * @author Michel Felipe <michelphp@gmail.com>
	 * @param object settings Objeto contendo as configurações do listBox. As seguintes propriedades podem ser definidas:
	 * 
	 * -------- Propriedades do objeto 'settings' ----------
	 * 
	 * btnAdd:      String contendo o id ou a classe do botão que executa a ação de adicionar um elemento
	 * 		        ao select
	 * 
	 * overlayOpen: Booleano true, caso o desenvolvedor utilize o plugin 'overlay' do jQuery jutamente com o listBox
	 * 
	 * inputAdd:    String contendo o id ou a classe do input do tipo texto no qual o usuário 
	 * 			    digita o valor a ser adicionado no select
	 * 
	 * datepicker:  Booleano true, caso o campo de texto seja do tipo data
	 * 
	 * elementList: String contendo o id ou a classe do elemento select. Este armazena os valores em uma lista
	 * 				para envio destes através de uma requisição ajax
	 * 
	 * btnRemove:   String contendo o id ou a classe do botão que executa a ação de remover um elemento da lista
	 * 
	 * @param object ajaxParams Objeto contendo propriedades da requisição ajax
	 *
	 * -------- Propriedades do objeto 'ajaxParams' ----------
	 * 
	 * btnSave: 	  String contendo o id ou a classe do botão que executa a ação de enviar os dados a serem
	 * 				  salvos no db
	 * 
	 * pageRequest:	  Página de envio dos dados a serem salvos no db
	 * 
	 * parentImgLoad: String contendo o id ou a classe do elemento pai no qual a imagem "carregando" será adicionada
	 * 
	 * diversParams:  Parâmetros adicionais a serem enviados juntamente com os valores da lista no select.
	 * 				  Estes são informados pelo desenvolvedor
	 * 
	 */
	listBox:function(settings,ajaxParams){
		var queueDelete = [];
		var queueInsert = [];
		var queueChange = [];
		var msgNotSave = ('msgNotSave' in settings)?settings.msgNotSave:'Por favor, selecione um dos novos elementos da lista antes de salvar';
		
		//Copia do objeto sem referência para utilização dos seus valores na ação "change" ao clicar em algum valor do listBox
		var bkpeditParams = jQuery.extend(true,{},ajaxParams.editParams);
		var regQueryEmpty = /\[\]/;
		
		var addQueue = function(settings,callbackQueue){
					if(typeof settings.jsonTest == 'string'){
						callbackQueue();
					}else{
	                    for(var w in settings.jsonTest){
	                        for (var v in settings.jsonTest[w]){
	                            switch (settings.operatorTest){
	                             case '!=':
	                                if(settings.jsonTest[w][v] != settings.valueTest){
	                                   callbackQueue();
	                                   
	                                   //Resolução de inconsistência - o objeto adicionado as queues estavam com valores repetidos
	                                   return false;
	                                }
	                             break;
	                             case '==':
	                                if(settings.jsonTest[w][v] == settings.valueTest){
	                                   callbackQueue();
	                                }
	                             break;
	                             default:
	                                null;
	                             break;
	                           } 
	                       }
	                    }
					}
                };
		/**
		 * Abre o listBox dentro de um overlay, a escolha do desenvolvedor
		 * @see jQuery.overlay Plugin overlay do jquery.tools 
		 */
		if(settings.overlayOpen){
			$('img[rel]').overlay({expose:{
                color:'#444',
                loadSpeed:100,
                opacity:0.6,
                zIndex:9995
            },
	         color:'#ccc',
	         top:40,
	         oneInstance:false,
	         api:true,
	         closeOnClick:false});
		}
		
		switch (settings.datepicker){
			case true:
				var jQueryParams = 'jQueryParams' in configs;
				if(!jQueryParams){
					/**
					 * @todo Efetuar requisição de parâmetros jquery no servidor,
					 * 		 caso não não esteja definido no js 
					 */
					configs['jQueryParams'] = {datePicker:{}};
				}
				$(settings.inputAdd).datepicker(configs.jQueryParams.datePicker);
				break;
			default:
				null;
			break;
		}
		var objSelect = $(settings.elementList).get(0);
		var _data = [];
        var _rsJson = [];
		
        if(settings.action == 'edit'){
			$(settings.btnEdit).click(function(){
				
			 $(ajaxParams.parentImgLoad).append($(configs.loadImg));
			
				if('editValueSend' in ajaxParams.editParams){
					ajaxParams.editParams[ajaxParams.editParams['keyEdit']] = ajaxParams.editParams['editValueSend'];
					bkpeditParams[bkpeditParams['keyEdit']] = ajaxParams.editParams['editValueSend'];
				}else{
					ajaxParams.editParams[ajaxParams.editParams['keyEdit']] = $(this).attr('value');
					bkpeditParams[bkpeditParams['keyEdit']] = $(this).attr('value');
				}
				
				var paramsEdit = {dataJson:JSON.stringify(ajaxParams.editParams)};
				$.post(configs.baseUrl+ajaxParams.pageRequestEdit,
						paramsEdit,
						function(data){
							_rsJson = '';
							if(!regQueryEmpty.test(data)){
								_rsJson = JSON.parse(data);
							}
							rsData = $.createOptions(settings.elementList,_rsJson,true);
							('onCompleteDataLoad' in settings)?settings.onCompleteDataLoad(rsData,_rsJson):null;
							$('.loadImg').remove();
						}
				);
			
			});
        }
		
		$(settings.btnAdd).click(function(){
			var objOpt = document.createElement('option');       
			
			objOpt.value = $(settings.inputAdd).val();
			objOpt.innerHTML = $(settings.inputAdd).val();
			
			//Não adiciona novos elementos caso o valor já exista na listBox
			for(var e in objSelect.options){
				if(objSelect.options[e].text == objOpt.value){
					alert('Este valor ja foi adicionado');
					return false;
				}
			}
			
            if(objSelect.length >0 && objOpt.value == objSelect.options[objSelect.options.length-1].text){
                alert('Este valor ja foi adicionado');
            }else if(objOpt.value == ''){
            	alert('Por favor, informe um valor');
            
            }else{
                    $(settings.elementList).append($(objOpt));

                    //Monta fila de valores a serem inseridos
                   addQueue({jsonTest:_rsJson,operatorTest:'!=',valueTest:objOpt.value},function(){
                        if(queueInsert.length >0){
                            queueInsert[queueInsert.length] = {dataInsert:{listBoxValue:objOpt.value},anotherParams:ajaxParams.editParams};
                        }else{
                            queueInsert = [{dataInsert:{listBoxValue:objOpt.value},anotherParams:ajaxParams.editParams}];
                        }
                        	
                    });
                   	
                    //Callback que captura o ultimo valor modificado no listbox
                    if('onChangeDataLoad' in settings){
                    	var strLastValue = $(settings.elementList+' option:last').get(0).text;
                    	settings.onChangeDataLoad(strLastValue,settings.elementList);
                    }
                 }
             });
		
		$(settings.btnRemove).click(function(){
			
			/* Remove uma opção do listBox e a informação vinculada a ele pelo atributo 'rel' em qualquer outro elemento
			 * Caso retorne uma exceção, exibe uma mensagem de que é necessário selecionar um elemento antes de remover
			 */
			try{
				var objOptRemoved = $(objSelect.options[objSelect.selectedIndex]).remove();
			}catch (e) {
				alert('Por favor, selecione um elemento da lista para remover');
				return false;
			}
            if($(objOptRemoved).val() == $(settings.otherElement).attr('rel')){
            	$(settings.otherElement).get(0).value = '';
            }

            //Monta a fila de valores a serem excluídos
            addQueue({jsonTest:_rsJson,operatorTest:'==',valueTest:$(objOptRemoved).val()},function(){
                if(queueDelete.length >0){
                    queueDelete[queueDelete.length] = {dataDelete:$(objOptRemoved).val(),anotherParams:ajaxParams.editParams};
                }else{
                    queueDelete = [{dataDelete:$(objOptRemoved).val(),anotherParams:ajaxParams.editParams}];
                }
            });
             for (var x in queueInsert) {
                   if($(objOptRemoved).val() == queueInsert[x]['dataInsert']){
                        queueInsert.splice(x,1);
                    }
                }
           
             //Callback que captura o ultimo valor modificado no listbox
             if('onChangeDataLoad' in settings){
             	var strLastValue = $(settings.elementList+' option:last').get(0).text;
             	settings.onChangeDataLoad(strLastValue,settings.elementList);
             }
        });
		
		$(settings.elementList).change(function(){
			
			if(queueChange.length == 0){
				queueChange[queueChange.length] = {anotherParams:bkpeditParams};
			}
			
			for(var x in queueChange){
				
				queueChange[x]['anotherParams'].valueChange = $(this).val();
				var paramsSend = JSON.stringify(queueChange[x]['anotherParams']);
				
				$(ajaxParams.parentImgLoad).append($(configs.loadImg));
				$(settings.otherElement).get(0).value = '';
				
				//alert(paramsSend);
				$.post(configs.baseUrl + ajaxParams.pageRequestOtherData,
						{paramsSend:paramsSend},
					   function(data){
							var rsJson = JSON.parse(data);
							
							$(settings.otherElement).attr('rel',queueChange[x]['anotherParams'].valueChange);
							if(rsJson.length > 0){
								for(var w in rsJson){
									$(settings.otherElement).get(0).value = rsJson[w].valOtherElement;
								}
							}else{
								$(settings.otherElement).get(0).value = '';
							}
							$('.loadImg').remove();
							
					   }
				);
				
			}
		});
	
		
		if(ajaxParams){
			$(ajaxParams.btnSave).click(function(){
				
				//Checa se existe um outro elemento relacionado com o listBox. Caso exista, checa se algum elemento da lista foi selecionado antes de salvar
				if('otherElement' in settings){
					if($(objSelect).val() === null  || (queueDelete.length == 0 && queueInsert.length == 0)){
						if(queueDelete.length == 0){
							alert(msgNotSave);
							return false;
						}
					}
					
					//Se ja foi adicionado algo a lista, checa se o valor selecionado é igual ao valor inserido na listBox
					if(queueInsert.length > 0){
						for(var q in queueInsert){
							if($(objSelect).val() != queueInsert[q]['dataInsert']['listBoxValue']){
								alert(msgNotSave);
								return false;
							}
						}
					}
				}
				
				//Campos do listBox ou relacionados com o mesmo que não podem ser vazios
				for(var x in settings.requiredFields){
					if($(settings.otherElement).get(0).name == x && $(settings.otherElement).val() == '' && queueDelete.length == 0){
						$('#'+x).attr('class','error').focus();
						alert(settings.requiredFields[x]);
						return false;
					}else{
						for(var y in queueInsert){
							if(queueInsert[y]['dataInsert'].listBoxValue == $(settings.otherElement).attr('rel')){
								queueInsert[y]['dataInsert']['otherValue'] = $(settings.otherElement).val();
							}
						}
						$('#'+x).attr('class','');
					}
				}
				
                $(ajaxParams.parentImgLoad).append($(configs.loadImg));
                                
				var tmpData = {};
				
				if(queueDelete.length > 0 || queueInsert.length > 0){
					if(queueDelete.length > 0){
						_data = queueInsert.concat(queueDelete);
					}else{
						_data = queueInsert;
					}
				}else{
					for(var x=0;x<objSelect.length;x++){
						
						for(var y in ajaxParams.diversParams){
							tmpData[y] = ajaxParams.diversParams[y];
						}
	                    tmpData[objSelect.name+'['+x+']'] = objSelect.options[x].text;
	                    _data[0] = tmpData;
					}
				}
				
				var strParams = JSON.stringify(_data);
				if(strParams.length <=3){
					alert('Por favor, adicione algum valor para salvar');
					$('.loadImg').remove();
					return false;
				}
				
				$.post(configs.baseUrl+ajaxParams.pageRequest,
					   {dataJson:strParams},
					   function(data){
							var rsJson = JSON.parse(data);
		                    $('.loadImg').remove();
		                    alert(rsJson.msg);
		                    
		                    if('onComplete' in settings){
		        				if(rsJson.msg){
		        					settings.onComplete(settings.paramsComplete);
		        				}
		        			}
					  });
			});
		}
	}
});
