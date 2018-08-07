
function addHandlers(root) {
  if (root == null)  root = $(document);

  if ( root.find(".datepicker") == null || root.find(".datepicker") == undefined)
     return;
     
   var inputs = document.getElementsByTagName("input");
   
   var z = 0;
//   $(document).ready(function() { 
//   for ( z = 0; z< inputs.length; z++)
//   {
//     
//       if ( inputs[z].className != null &&  inputs[z].className.indexOf("datepicker") > -1 )
//       {

//                 alert(  inputs[z].id );
//		          $('#'+inputs[z].id).datepicker();	
//	         
//       }
//   
//   }  
//   });
  
try {
$('.datepicker').datepicker('destroy').datepicker();
 } catch(exp){}

$('.datepicker').removeClass('hasDatepicker').datepicker();
//  if ( root.find(".datepicker").length > 0) {  

//          root.find(".datepicker").datepicker({
//            showOn: 'focus'
//          });
//  
//  }
} 




function getOnClick( img ){


    if ( img == null )
	   return "";
	
	
	
	var out = outerHTML( img );
	
	if ( out.indexOf("onclick") < 0 )
	  return "";
	
	var ar = out.split("onclick=");
	
	//alert( ar );
	var sub = ar[1].split('"');
	
	var funcao = sub[1];
	
	var ar = funcao.split("(");
	
	var tmpfunc = ar[1].split(")");
	
	var parametros = tmpfunc[0].split(",");
	
	addGrupo( replaceAll(parametros[0],"'",""),replaceAll(parametros[1],"'",""),replaceAll(parametros[2],"'",""),replaceAll(parametros[3],"'",""),replaceAll(parametros[4],"'","") );
	
	
	//execute( sub[1] );

}

function replaceAll(string, token, newtoken) {
	while (string.indexOf(token) != -1) {
 		string = string.replace(token, newtoken);
	}
	return trim(string);
}

function trim(str) {
		return str.replace(/^\s+|\s+$/g,"");
	}



function pega_hidden( nome , form) {

   for ( i = 0; i < form.elements.length; i++){
   
    if ( form.elements[i].type != "hidden")
        continue;
   
   
    //  alert( form.elements[i].name + " - " +  form.elements[i].type + " - " + nome.indexOf(form.elements[i].name) + " - " + nome);
         if ( form.elements[i].name != null && nome.indexOf(form.elements[i].name) >-1 && form.elements[i].type == "hidden"){
              return form.elements[i];
         }
   }
   return null;;
}

function addGrupo()
{
try {
$('.datepicker').datepicker('destroy').datepicker();
 } catch(exp){}
     var hd = document.getElementById("hd_"+ arguments[1].replace("_SQ_1_","")+"");
       
      //var hd = pega_hidden( "hd_"+ arguments[1].replace("_SQ_1_","") , document.forms[0] );
      
    //   alert( arguments[1] + "hd_"+ arguments[1].replace("_SQ_1_","")+" - "+ hd); 
    if ( arguments[4] == "v")
      {
         // alert(arguments.length);
         var td = document.createElement("TD");
         var tdM = document.getElementById("td_"+ arguments[1]+"");
           
         var tr = document.getElementById("tr_"+ arguments[1].replace("_SQ_1_","")+"");
     
      
        // alert(hd);
         if ( tdM == null || hd == null || tr == null)
           return;
       }
       else
       {
         var tdM = document.getElementById("tr_"+ arguments[1]+"");
         var td = document.getElementById("tbody_"+ arguments[1].replace("_SQ_1_","")+"");
        // alert( td + " - " + "tr_"+ arguments[1] );
       } 
       
       
       var max = parseInt(hd.value);
       max++;
       //alert(arguments[3]+ " "+ max);
       var texto = "";
    if ( arguments[4] == "v") //Criaremos uma linha na vertical
      { 
       texto = replaceTotal( tdM.innerHTML , "SQ_1_","SQ_"+max+"_");
       
       //Substituindo alguns parametros de imagens para agora ele funcionar na exclusão
       //E garantindo nomes únicos para cada campo
       texto =  replaceTotal( texto , "hd_", "_hd_");
       texto =  replaceTotal( texto , arguments[0], arguments[3]+ " "+ max);
       texto =  replaceTotal( texto , " . 1 ", " . "+max);
       texto =  texto.replace(arguments[0], arguments[3]+ " "+ max);
       texto =  texto.replace("add2.png", "delete.png");
       texto = texto.replace("addGrupo","removeGrupo");
       
         td.innerHTML = texto;
           // alert(td.innerHTML);
           td.id = tdM.id.replace("SQ_1_","SQ_"+max+"_");
          // alert(td.id);
           tr.appendChild(td);
       }
       else
       {
          var trp = document.createElement("TR");
           trp.className = "trbody";
          for (z=0;z< tdM.cells.length;z++)
	      {
	          texto = replaceTotal ( tdM.cells[z].innerHTML , "SQ_1_","SQ_"+max+"_");
     
                texto =  replaceTotal( texto , "hd_", "_hd_");
                
               texto =  replaceTotal( texto , arguments[0], arguments[3]+ " "+ max);
               //texto =  texto.replace(arguments[0], arguments[3]+ " "+ max);
               texto =  replaceTotal ( texto , " . 1 ", " . "+max);
//               texto =  replaceTotal ( texto , " hasDatepicker", "");
              
               
               texto =  texto.replace("add2.png", "delete.png");
               texto = texto.replace("addGrupo","removeGrupo");
               
               var frag = getSeriaJquerie(texto);
               
               texto =  replaceTotal ( texto , 'jQuery' + frag +'"1"' , "");
               texto =  replaceTotal ( texto , 'jQuery' + frag +'"2"' , "");
               texto =  replaceTotal ( texto , 'jQuery' + frag +'"3"' , "");
               texto =  replaceTotal ( texto , 'jQuery' + frag +'"4"' , "");
               texto =  replaceTotal ( texto , 'jQuery' + frag +'"5"' , "");
	     
	          var tdp = document.createElement("TD");
	          tdp.innerHTML = texto;
	        
	          trp.appendChild(tdp);
	          trp.id  = tdM.id.replace("SQ_1_","SQ_"+max+"_");
	         
	          //alert(texto); alert(getSeriaJquerie(texto));
	       }
	       td.appendChild(trp);
       
       }
       
       hd.value = max.toString();
      
      
      var arf  = arguments[1].split('_');
      //Limpa os novos campos que foram acabados de criar na nova linha.
      var arNovos = localiza( new Array(arf[0]+"_",arf[1]+"_"+arf[2]+"_"+max),false);
      //alert(arNovos.length);
      for (z=0;z< arNovos.length;z++)
	      {
	          arNovos[z].value = "";
	      }
              
              var txt = "tr_"+arf[0]+"_"+arf[1]+"_SQ_"+max.toString()+"_";
              var tr_nova_linha = document.getElementById(txt);
             // alert( txt );
             // alert( tr_nova_linha );
              if ( tr_nova_linha != null ){
                  //alert( tr_nova_linha.getElementsByTagName( "a") );
                  var  links = tr_nova_linha.getElementsByTagName( "a");
                  //alert( links );
                  if ( links.length > 0 ){
                      links[0].innerText = "[Selecionar]";
                        $(document).ready(function(){
                             // $(links[0].id).colorbox({iframe:true, innerWidth: "85%", height:"86%"});
                              
                               $(".iframe_modal").colorbox({iframe:true, innerWidth: "85%", height:"86%"});

                        });

                     

                      //  alert( links[0].innerText + " - " + max );
                  }
              }
              
              
	     addHandlers();   
	          
}

function getSeriaJquerie(txt)
{
    var arr = txt.split("jQuery");
    
    if ( arr.length > 1)
    {
    
        var arr2 =  arr[1].split('"');  return arr2[0];

    }
    else
    return "";
  }


function outerHTML(node){
    // if IE, Chrome take the internal method otherwise build one
  return node.outerHTML || (
      function(n){
          var div = document.createElement('div'), h;
          div.appendChild( n.cloneNode(true) );
          h = div.innerHTML;
          div = null;
          return h;
      })(node);
  }


function SubstituiInputs(div, strComp) { 

div = typeof div === "string" ? document.getElementById(div) : div; 

var elms = div.getElementsByTagName("*");

//alert( div.innerHTML );
//alert( elms.length );
 for(var i = 0, maxI = elms.length; i < maxI; ++i) 
 { var elm = elms[i]; 
 
     if ( elm != null && elm.id != null && elm.id != ""){
     
     } else{
       continue;
     }
 
 
      var str_htm = outerHTML( elm );
       obj = document.getElementById(elm.id);
       
      // elm = obj;
          switch(elm.type) { 
           case "hidden":      
              strComp = "&nbsp;" + strComp.replace(str_htm, ""); 
           case "text":      
              strComp = "&nbsp;" + strComp.replace(str_htm, obj.value);      
           case "textarea":                
              //alert( elm.outerHTML + " -- " + elm.value +" --" + str_htm + "--- " + obj.value);
              //strComp =strComp.replace(elm.outerHTML, elm.value);
              strComp = "&nbsp;" + strComp.replace(str_htm, obj.value);
              break;
//            case "image": 
//           case "button": 
//           case "reset": 
//           case "submit": 
//           case "file":
//            case "hidden":
//             case "password":
//              case "radio": 
              case "checkbox":
              var sr_res = "Não";
              if ( obj.checked )
                 sr_res = "Sim";
                 
              strComp =strComp.replace(str_htm, sr_res  );
              
              break;
               case "select-one": 
               case "select-multiple":
               
               var txt = "";
               
               if ( obj.selectedIndex > -1 ){
                 txt = obj.options[ obj.selectedIndex ].text;
               
               }
               if ( txt.toUpperCase().indexOf("SELECIONE --") > 0 ){
                   txt = "";
               } 
              
              //strComp =strComp.replace(elm.outerHTML, txt);
              
              strComp =strComp.replace(str_htm, txt);
              
              break;          
              

                    //alert("Type: " + elm.type + "\nName: " + elm.name + "\nId: " + elm.id);
                } 
     }
     
    return  strComp; 
 } 


function removeGrupo()
{
  if ( arguments[4] == "v")
      {
     var tdM = document.getElementById("td_"+ arguments[1]+"");
     }
   else
     {
      
         var tdM = document.getElementById("tr_"+ arguments[1]+"");
       
     } 
     try
     {
     
   //  var d = document.getElementById('myDiv');
   //var olddiv = document.getElementById(divNum);
  //d.removeChild(olddiv);
  
      
     
      tdM.parentNode.removeChild(tdM); 
         //tdM.removeNode(true);
         var arf = arguments[1].split("_");
         var arNovos = localiza( new Array(arf[0]+"_",arf[1]+"_"+arf[2]+"_1"),false);
     // alert(arNovos.length);
      for (z=0;z< arNovos.length;z++)
	      {
	          //recalculando tudo....arNovos[z].value = "";
	          arNovos[z].focus();
	          arNovos[z].blur();
	      }
        
        
      } catch(ex){}
     
   
}

function Arredonda( valor , casas ){
	
   var novo = Math.round( valor * Math.pow( 10 , casas ), casas ) / Math.pow( 10 , casas );

   return( novo );

}
function calculaColunaCronograma(nome, ano  ){
    
       var arr = localiza( new Array("CRONO_","_ANO"+ano), false, "_TLT");
       var total = 0;
       
       for ( var i = 0; i < arr.length; i++ ){
           
           var sub_total = getNum( arr[i] );
           
           total += sub_total;
       }
    
    
       //var div_total = localiza( new Array("div_", "CRONO_","_ANO"+ano,"_TLT"), true, null );
       var hd_total = localiza( new Array("CRONO_","_ANO"+ano,"_TLT"), true,"div_" );
       //alert( hd_total );
       if ( hd_total != null ){
          
         var div_total = document.getElementById("div_"+ hd_total.id); 
         div_total.innerHTML = numeroEmReal( total );
         hd_total.value = numeroEmReal( total );
       }
       
}

function calculaLinhaCronograma(nome, id_acao  ){
    
       var arr = localiza( new Array("CRONO_","_A"+id_acao), false, "_TLT");
       var total = 0;
       for ( var i = 0; i < arr.length; i++ ){
           
           var sub_total = getNum( arr[i] );
           
           total += sub_total;
       }
    
      // var div_total = localiza( new Array("CRONO_","_A"+id_acao,"_TLT","div_"), true );
       var hd_total = localiza( new Array("CRONO_","_A"+id_acao,"_TLT"), true,"div_" );
       
       if ( hd_total != null ){
           
         var div_total = document.getElementById("div_"+ hd_total.id); 
         div_total.innerHTML = numeroEmReal( total );
         hd_total.value = numeroEmReal( total );
       }
       
       //Total geral de baixo...
        var arr = localiza( new Array("CRONO_","_ANO","_TLT"), false,"GRL_");
       var total = 0;
       for ( var i = 0; i < arr.length; i++ ){
           
           var sub_total = getNum( arr[i] );
           
           total += sub_total;
       }
       
          var hd_total = localiza( new Array("CRONO_","_TLT","GRL"), true );
       
       if ( hd_total != null ){
           
         var div_total = document.getElementById("div_"+ hd_total.id); 
         div_total.innerHTML = numeroEmReal( total );
         hd_total.value = numeroEmReal( total );
       }
       
}


function calculaCronograma(){
    
   var str = arguments[0];
    //Nome..
   //1 - Objeto
   //2 - Ano
   //3 - Ação
   var ar = str.split('_');
    
    
    calculaColunaCronograma( str, arguments[2]);
    calculaLinhaCronograma( str, arguments[3]);
    
    
    
}


//Calculo de total
function calculatotal()
{
   var str = arguments[0].name;
   var ar = str.split('_');
   //F1 G1 G1 SQ 1 C4 CLC
   
   // Calculo Horizontal..
   var txtHtotal = document.getElementById(ar[0]+"_"+
                    ar[1]+"_"+ar[2]+"_0_"+ar[4]+"_"+
                    ar[5]+"_TLT");
   var hd = document.getElementById("hd_"+ar[0]+"_"+
                    ar[1]);

//   alert(txtHtotal + "-"+ str + "---"+ ar[0]+"_"+
//                    ar[1]+"_"+ar[2]+"_0_"+ar[4]+"_"+
//                    ar[5]+"_TLT");

   if ( arguments.length > 3 )
   {       //txtHtotal = localiza ( new Array("C"+ arguments[3]),false )[0];
   }
   if ( txtHtotal != null && hd != null )
    {
       var max = parseInt(hd.value);
       var totalH = 0;
       for (i=1; i <= max; i++)
        {
          var tx = document.getElementById(ar[0]+"_"+
                    ar[1]+"_"+ar[2]+"_"+i+"_"+ar[4]+"_"+
                    ar[5]+"");


           if ( tx == null)
              continue;
        
        
           var num = getNum( tx );
         
           if ( num.toString() != "NaN")
           {
                totalH += num;
           }
        }
        
        txtHtotal.value = numeroEmReal(totalH);
        //alert( txtHtotal.value );
        //txtHtotal.focus(); txtHtotal.blur();
        //Se ele não possuir um campo de total em outro grupo, então podemos encerrar aqui..
       
    
    }
    
}


function getNum(tx)
{
    var totalH = 0;
    var stx = replaceTotal( tx.value , ".","");
    
    stx = parseFloat( replaceTotal( stx , ",","."));
           if ( stx.toString() != "NaN")
           totalH += stx;
     return     totalH;  
}


function formataNumBr(tx)
{
       var stx = "";
       
       stx = replaceTotal( tx , ".","|");
       stx =  replaceTotal( stx , ",",".");
       
       stx = replaceTotal( stx , "|",",");
            
           
     return     stx;  
}


function getNumBr(tx)
{
    var totalH = 0;
    //var stx = replaceTotal( tx , ".",",");
    
    var stx = parseFloat( replaceTotal( tx , ",","."));
    
           if ( stx.toString() != "NaN")
           totalH += stx;
    
    stx = replaceTotal( totalH.toString() , ".",",");
            
           
     return     stx;  
}


function desabilitaimagens()
{
   var ar = document.getElementsByTagName("img");
     
   for (i=0; i <= ar.length; i++)
        {
            if ( ar[i] == null || ar[i].name == null || 
			       ar[i].id == undefined)
			        continue;
			        
			        if ( ar[i].id.indexOf("img_") > -1 )
			            ar[i].style.display = "none";
			        
        }

}


function localiza(arrfiltro, retorna1, exceto )
{

    var arp = document.getElementsByTagName("input");
   var ars = document.getElementsByTagName("select");
   var arst = document.getElementsByTagName("textarea");
   var ar_divs = document.getElementsByTagName("div");
   
   var ar = new Array();
   var arr = new Array();
 for (gz = 0 ; gz <= 2 ; gz++)
 {
  if ( gz == 0)
       ar = ars;
  
  if ( gz == 1)
       ar = arp;
   
  if ( gz == 2)
       ar = arst;
   
   
  if ( gz == 3)
       ar = ar_divs;
       
   for (i=0; i <= ar.length; i++)
        {
        
			retorna = false;
			 for (z =0; z < arrfiltro.length ; z++)
			 {
			
			    if ( ar[i] == null || ar[i].name == null || 
			       ar[i].name == undefined)
			        continue;
			 
			   if ( exceto != null && ar[i].name.indexOf( exceto ) > -1)
			      continue;
			 
				if ( ar[i].name.indexOf( arrfiltro[z] ) > -1)
				   retorna = true;
				else
				{
				   retorna = false;
				   break;
				}

		     }
			if (retorna == true && retorna1)
				return ar[i];
			
			if (retorna == true && !retorna1)
			    arr[  arr.length ] = ar[i];
		    // if ( strpos($key,
			
		}
	}
		
		
		if ( !retorna1)
		    return arr;
		else
		    return null;
   
}

function robj(nome)
{
   return document.getElementById(nome);
}
function numeroEmReal(total)
{
   var exibe = CurrencyFormatted(  total.toString() );
   exibe = CommaFormatted(  exibe );
 
   return  formataNumBr( exibe  ) ;
}

function bloqueiaCampos(form, bloq, exceto, imagens)
{
 if ( bloq == null)
    bloq = false;
    var i;
    
    for ( i = 0; i < form.elements.length; i ++)
     {
      if ( form.elements[i].type != null && 
           ( ( form.elements[i].type.indexOf("button") > -1 ) 
           || form.elements[i].type.indexOf("hidden") > -1 )
           )
           {  
              continue;
           }
     
       try{
         
       
          if ( exceto == null || 
               ( form.elements[i].name != null && 
                 exceto.indexOf("|"+form.elements[i].name +"|") < 0 ) ) 
                 {
       
                   form.elements[i].disabled = (bloq == true);
                 }
       }catch(exp){}
     
     }
     return;
     if ( imagens != null )
     {
        	 var imgs =  document.getElementsByTagName("img");
			 for ( i = 0; i < imgs.length; i ++)
              { 
               if ( exceto == null || 
               ( imgs[i].id != null &&  
                 exceto.indexOf("|"+imgs[i].alt +"|") < 0 ) && imgs[i].alt != "" ) 
                 {
                   if ( bloq )
                       imgs[i].style.display="none";
                   else
                       imgs[i].style.display="";
                   
                          
                 }
              
              }
     
     }
}

function existeClasse(classe, valor){
    
    var ar = classe.split(' ');
    
    for ( var i = 0; i < ar.length; i++ ){
        if ( ar[i] == valor )
            return true;
    }
    
    return false;
}


function verificaPreenchimento( obj ){
    
    if ( obj.value == ""){
         if ( ! existeClasse(obj.className, "campo_obrigatorio") ){
             obj.className += " campo_obrigatorio";
         }
    }else{
         if (  existeClasse(obj.className, "campo_obrigatorio") ){
             
          obj.className = obj.className.replace(" campo_obrigatorio","");
          obj.className = obj.className.replace("campo_obrigatorio","");
         }
    }
    
    
}