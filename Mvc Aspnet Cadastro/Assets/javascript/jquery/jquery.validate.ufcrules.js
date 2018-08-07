/**
 * Validações adicionais para o plugin validate do JQuery
 */
jQuery.validator.addMethod("notEqual", 
							function(value, element, params) {
								return this.optional(element) || value != params;
							}, 
						   "Selecione um valor.");
