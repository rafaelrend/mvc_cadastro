
/*
Tipr 2.0.1
Copyright (c) 2015 Tipue
Tipr is released under the MIT License
http://www.tipue.com/tipr
*/


(function($) {

     $.fn.tipr = function(options) {
     
          var set = $.extend( {
               
               'speed'        : 200,
               'mode'         : 'bottom'
          
          }, options);

          return this.each(function() {
          
               var tipr_cont = '.tipr_container_' + set.mode;

               $(this).hover(
                    function ()
                    {
                         var d_m = set.mode;

                         if ($(this).attr('data-mode'))
                         {
                              d_m = $(this).attr('data-mode')
                              tipr_cont = '.tipr_container_' + d_m;   
                         }
                         var add_class = "";
                          if ($(this).attr('data-add-class'))
                         {
                              add_class = " "+ $(this).attr('data-add-class');
                         }
                         var out = '<div class="tipr_container_' + d_m + '"><div class="tipr_point_' + d_m + '"><div class="tipr_content'+add_class+'">' + $(this).attr('data-tip') + '</div></div></div>';
                         
                         $(this).append(out);
                    
                         var w_t = $(tipr_cont).outerWidth();
                         var w_e = $(this).width();
                         var m_l = (w_e / 2) - (w_t / 2);
                         
                         if ( d_m == "right"){
                             m_l = 0;
                         }
                         //
                         //alert( set.mode );
                    
                         $(tipr_cont).css('margin-left', m_l + 'px');
                         $(this).removeAttr('title alt');
                         
                         $(tipr_cont).fadeIn(set.speed);              
                    },
                    function ()
                    {   
                         $(tipr_cont).remove();    
                    }     
               );
                              
          });
     };
     
})(jQuery);
