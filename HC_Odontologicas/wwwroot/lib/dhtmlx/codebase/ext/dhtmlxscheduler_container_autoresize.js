/*

@license
dhtmlxScheduler v.5.3.4 Stardard

To use dhtmlxScheduler in non-GPL projects (and get Pro version of the product), please obtain Commercial/Enterprise or Ultimate license on our site https://dhtmlx.com/docs/products/dhtmlxScheduler/#licensing or contact us at sales@dhtmlx.com

(c) XB Software Ltd.

*/
Scheduler.plugin(function(e){!function(){function t(){a=!1,e.callEvent("onAfterSchedulerResize",[]),a=!0}e.config.container_autoresize=!0,e.config.month_day_min_height=90,e.config.min_grid_size=25,e.config.min_map_size=400;var i=e._pre_render_events,a=!0,n=0,r=0;e._pre_render_events=function(t,s){if(!e.config.container_autoresize||!a)return i.apply(this,arguments);var o=this.xy.bar_height,d=this._colsS.heights,_=this._colsS.heights=[0,0,0,0,0,0,0],l=this._els.dhx_cal_data[0]
;if(t=this._table_view?this._pre_render_events_table(t,s):this._pre_render_events_line(t,s),this._table_view)if(s)this._colsS.heights=d;else{var h=l.firstChild;if(h.rows){for(var c=0;c<h.rows.length;c++){if(++_[c]*o>this._colsS.height-this.xy.month_head_height){var u=h.rows[c].cells,g=this._colsS.height-this.xy.month_head_height
;1*this.config.max_month_events!==this.config.max_month_events||_[c]<=this.config.max_month_events?g=_[c]*o:(this.config.max_month_events+1)*o>this._colsS.height-this.xy.month_head_height&&(g=(this.config.max_month_events+1)*o);for(var f=0;f<u.length;f++)u[f].childNodes[1].style.height=g+"px";_[c]=(_[c-1]||0)+u[0].offsetHeight}_[c]=(_[c-1]||0)+h.rows[c].cells[0].offsetHeight}_.unshift(0),h.parentNode.offsetHeight<h.parentNode.scrollHeight&&h._h_fix
}else if(t.length||"visible"!=this._els.dhx_multi_day[0].style.visibility||(_[0]=-1),t.length||-1==_[0]){var v=(h.parentNode.childNodes,(_[0]+1)*o+1);r!=v+1&&(this._obj.style.height=n-r+v-1+"px"),v+="px",l.style.top=this._els.dhx_cal_navline[0].offsetHeight+this._els.dhx_cal_header[0].offsetHeight+parseInt(v,10)+"px",l.style.height=this._obj.offsetHeight-parseInt(l.style.top,10)-(this.xy.margin_top||0)+"px";var m=this._els.dhx_multi_day[0];m.style.height=v,
m.style.visibility=-1==_[0]?"hidden":"visible",m=this._els.dhx_multi_day[1],m.style.height=v,m.style.visibility=-1==_[0]?"hidden":"visible",m.className=_[0]?"dhx_multi_day_icon":"dhx_multi_day_icon_small",this._dy_shift=(_[0]+1)*o,_[0]=0}}return t};var s=["dhx_cal_navline","dhx_cal_header","dhx_multi_day","dhx_cal_data"],o=function(t){n=0;for(var i=0;i<s.length;i++){var a=s[i],o=e._els[a]?e._els[a][0]:null,d=0;switch(a){case"dhx_cal_navline":case"dhx_cal_header":d=parseInt(o.style.height,10)
;break;case"dhx_multi_day":d=o?o.offsetHeight-1:0,r=d;break;case"dhx_cal_data":var _=e.getState().mode;if(d=o.childNodes[1]&&"month"!=_?o.childNodes[1].offsetHeight:Math.max(o.offsetHeight-1,o.scrollHeight),"month"==_){if(e.config.month_day_min_height&&!t){d=o.getElementsByTagName("tr").length*e.config.month_day_min_height}t&&(o.style.height=d+"px")}else if("year"==_)d=190*e.config.year_y;else if("agenda"==_){if(d=0,
o.childNodes&&o.childNodes.length)for(var l=0;l<o.childNodes.length;l++)d+=o.childNodes[l].offsetHeight;d+2<e.config.min_grid_size?d=e.config.min_grid_size:d+=2}else if("week_agenda"==_){for(var h,c,u=e.xy.week_agenda_scale_height+e.config.min_grid_size,g=0;g<o.childNodes.length;g++){c=o.childNodes[g];for(var l=0;l<c.childNodes.length;l++){for(var f=0,v=c.childNodes[l].childNodes[1],m=0;m<v.childNodes.length;m++)f+=v.childNodes[m].offsetHeight;h=f+e.xy.week_agenda_scale_height,
h=1!=g||2!=l&&3!=l?h:2*h,h>u&&(u=h)}}d=3*u}else if("map"==_){d=0;for(var p=o.querySelectorAll(".dhx_map_line"),l=0;l<p.length;l++)d+=p[l].offsetHeight;d+2<e.config.min_map_size?d=e.config.min_map_size:d+=2}else if(e._gridView)if(d=0,o.childNodes[1].childNodes[0].childNodes&&o.childNodes[1].childNodes[0].childNodes.length){for(var p=o.childNodes[1].childNodes[0].childNodes[0].childNodes,l=0;l<p.length;l++)d+=p[l].offsetHeight;d+=2,d<e.config.min_grid_size&&(d=e.config.min_grid_size)
}else d=e.config.min_grid_size;if(e.matrix&&e.matrix[_]){if(t)d+=0,o.style.height=d+"px";else{d=0;for(var x=e.matrix[_],b=x.y_unit,y=0;y<b.length;y++)d+=x._section_height[b[y].key]}d-=1}("day"==_||"week"==_||e._props&&e._props[_])&&(d+=2)}d+=1,n+=d}e._obj.style.height=n+"px",t||e.updateView()},d=function(){if(!e.config.container_autoresize||!a)return!0;var i=e.getState().mode;if(!i)return!0;var n=document.documentElement.scrollTop;if(o(),e.matrix&&e.matrix[i]||"month"==i){
(window.requestAnimationFrame||window.setTimeout)(function(){o(!0),document.documentElement.scrollTop=n,t()},1)}else t()};e.attachEvent("onBeforeViewChange",function(){var t=e.config.container_autoresize;if(e.xy.$original_scroll_width||(e.xy.$original_scroll_width=e.xy.scroll_width),e.xy.scroll_width=t?0:e.xy.$original_scroll_width,e.matrix)for(var i in e.matrix){var a=e.matrix[i];a.$original_section_autoheight||(a.$original_section_autoheight=a.section_autoheight),
a.section_autoheight=!t&&a.$original_section_autoheight}return!0}),e.attachEvent("onViewChange",d),e.attachEvent("onXLE",d),e.attachEvent("onEventChanged",d),e.attachEvent("onEventCreated",d),e.attachEvent("onEventAdded",d),e.attachEvent("onEventDeleted",d),e.attachEvent("onAfterSchedulerResize",d),e.attachEvent("onClearAll",d),e.attachEvent("onBeforeExpand",function(){return a=!1,!0}),e.attachEvent("onBeforeCollapse",function(){return a=!0,!0})}()});
//# sourceMappingURL=../sources/ext/dhtmlxscheduler_container_autoresize.js.map