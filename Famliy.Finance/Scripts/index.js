﻿
var index = function(){
	var opinionSearchedStock = [];
	var centerTimer;
	var parseData = function(str){
		if(typeof str == 'undefined' || str == null || str == 'null'){
			return '';
		}
		return str;
	};
	/**
	 * 按字节截取字符串，支持中文
	 * @param str 字符串
	 * @param len 长度
	 * @param hasDot 是否添加...
	 */
	var subCHNString = function(str, len, hasDot){
		var newLength = 0;
		var newStr = "";
		var chineseRegex = /[^\x00-\xff]/g;
		var singleChar = "";
		var strLength = str.replace(chineseRegex,"**").length;
		for(var i = 0;i < strLength;i++){
			singleChar = str.charAt(i).toString();
			if(singleChar.match(chineseRegex) != null){
				newLength += 2;
			} else {
				newLength++;
			}
			if(newLength > len) {
				break;
			}
			newStr += singleChar;
		}
		if(hasDot && strLength > len) {
			newStr += "...";
		}
		return newStr;
	};
	/**
	 * 特殊字符串转换
	 */
	var formatSpecialChar = function(str){
		if(str) {
			return str.replace(/\"/g,'&quot;').replace(/'/g,'&#39;');
		}
		return '';
	};
	return {
		init:function(){
			$('#searchOpinion').autocomplete({
				minLength:1,
				//autoFocus:true,
				source:function(request, response){
					$.ajax({
						cache: false,
						dataType: 'jsonp',
						url: APP_HOST+'autoCompleteStock.php',
						data: 'q='+encodeURIComponent(request.term)+'&s=pc',
						success: function(data){
							$('#aotoCompeleteDivId2').show();
							if(data.length > 0) {
								defaultLi = '<h3>' + request.term + '，搜索结果</h3>';
							} else {
								defaultLi = '<h3>对不起，找不到：</h3> <li>' + request.term.substr(0,10) + '</li>';
							}
							if($('#aotoCompeleteDivId2').find('h3').length > 0) {
								$('#aotoCompeleteDivId2').find('h3').next('li').remove();
								$('#aotoCompeleteDivId2').find('h3').remove();
							}
							$('#aotoCompeleteDivId2').prepend(defaultLi);
							response($.map(data,function(item){
								opinionSearchedStock.push(item.code);
								return {
									label:item.name,
									value:item.code
								};
							}));
						}
					});
				},
				select:function(event,ui){
					//var href = 'http://cj.gw.com.cn/SearchStock.php?code='+ui.item.value;
					//window.location.href = href;
					var val_tmp = ui.item.value;
					val_tmp = val_tmp.replace(/[A-Za-z]/g,'');
					$(this).val(val_tmp);
					return false;
				},
				focus:function(event,ui){
					var val_tmp = ui.item.value;
					val_tmp = val_tmp.replace(/[A-Za-z]/g,'');
					$(this).val(val_tmp);
					return false;
				},
				close:function(event, ui){
					$('#aotoCompeleteDivId2').hide();
				},
				open:function(event, ui){
					$('#aotoCompeleteDivId2').find('ul').css({top:"0px"});
				}
			})
			.autocomplete("option","appendTo","#aotoCompeleteDivId2")
			//.autocomplete( "option", "position", { my : "left top+24", at: "left bottom" } )
			.data( "autocomplete" )._renderItem = function( ul, item ) {
				//自定义样式
				return $("<li val="+item.value+">")
						.data("item.autocomplete", item)
						.append("<a href='javascript:void(0)'><em>"+item.value+"</em><span>"+item.label+"</span></a>")
						.appendTo(ul);
			};
			//点击查舆情按扭
			$('#publicSearch').click(function(){
				var stockCode = $('#searchOpinion').val();
				if(stockCode == '' || stockCode == '股票名称或代码'){
					setTimeout(function(){$('#public').hide();},500);
					return false;
				}
				
				$('#more_con').hide();
				$('.markets').hide();
				$("#main .centerblock .ads").hide();
				$('#public').html('<div class="loading"><img src="images/loadinfo.gif" alt="" /><br>正在加载，请稍后...</div>');
				$('#aotoCompeleteDivId2').hide();
				
				
				var isCustomStock = 1;
				if($.inArray(stockCode,opinionSearchedStock) != -1) {
					isCustomStock = 0;
				}
				$.ajax({
					type:'post',
					dataType:'json',
					data:{stockCode:stockCode,isCustomStock:isCustomStock},
					url:'stockOpinion.php',
					success:function(data){
						var strHtml = '';
						if(data.stock) {
							strHtml += '<div class="company">' + data.stock.name + ' ' + data.stock.code;
							if(data.stock.zd > 0) {
								strHtml += '<span class="price up">' + '<span>'+data.stock.lp+'</span><span>+'+data.stock.zd+' (元)</span><span>+'+data.stock.zf+'</span></span>';
							} else {
								strHtml += '<span class="price down">' + '<span>'+data.stock.lp+'</span><span>'+data.stock.zd+' (元)</span><span>'+data.stock.zf+'</span></span>';
							}
							strHtml += '</div>';
							strHtml += '<div class="title">热点舆情</div><ul class="stockList">';
							if(data.opinion.hotOpinion && data.opinion.hotOpinion.length > 0) {
								var hotOpinion = data.opinion.hotOpinion;
								for(var i in hotOpinion) {
									strHtml += '<li><a target="_blank" href="detail/opinionDetail.php?type='+hotOpinion[i].type+'&id='+hotOpinion[i].newsId+'" type="'+hotOpinion[i].type+'" newsId="'+hotOpinion[i].newsId+'">'+hotOpinion[i].title+'</a><br><span class="info">'+hotOpinion[i].source+' '+hotOpinion[i].date+'</span></li>';
								}
							} else {
								strHtml += '<li><a href="javascript:;" style="text-decoration: none;cursor:default;">该个股今日截至当前无相关舆情</a></li>';
							}
							strHtml += '</ul>';
							
							
							strHtml += '<div class="title">重点舆情</div><ul class="stockList">';
							if(data.opinion.clientOpinion && data.opinion.clientOpinion.length > 0) {
								var clientOpinion = data.opinion.clientOpinion;
								for(var i in clientOpinion) {
									strHtml += '<li><a target="_blank" href="detail/opinionDetail.php?type='+clientOpinion[i].type+'&id='+clientOpinion[i].newsId+'" type="'+clientOpinion[i].type+'" newsId="'+clientOpinion[i].newsId+'">'+clientOpinion[i].title+'</a><br><span class="info">'+clientOpinion[i].source+' '+clientOpinion[i].date+'</span></li>';
								}
							} else {
								strHtml += '<li><a href="javascript:;" style="text-decoration: none;cursor:default;">该个股今日截至当前无相关舆情</a></li>';
							}
							strHtml += '</ul>';
							//$('#more_con').hide();
							//$('.markets').hide();
							//$("#main .centerblock .ads").hide();
							
							$('#public').html(strHtml);
							//$('#aotoCompeleteDivId2').hide();
						} else {
							$('#public').html('<div class="loading">未查到相关个股或舆情数据...</div>');
						}
					},
					error:function(){
						$('#public').html('<div class="loading">未查到相关个股或舆情数据...</div>');
					}
				});
			});
			$('#searchOpinion').keydown(function(e){
				var evt = window.event || e;
				var keyCode = evt.keyCode ? evt.keyCode : evt.which ? evt.which : evt.charCode;
				if(keyCode == 13) {
					$('#publicSearch').trigger('click');
				}
			});
			//展开、隐藏中间栏
			$("#toggle_blue").click(function(){
				if ($("#toggle_blue").hasClass('close')) {
					centerTimer = setTimeout(function(){
						if($("#toggle_blue").hasClass('close')) {
							$('#more_con').show();
							$("#main .centerblock .ads").show();
						} else {
							$('#more_con').hide();
							$("#main .centerblock .ads").hide();
						}
						clearTimeout(centerTimer);
						$('.scroll-pane').jScrollPane();
					},400);
				} else {
					$('#more_con').hide();
					$('.markets').show();
					$("#main .centerblock .ads").hide();
					$('#searchOpinion').val('股票名称或代码').css({"color": "#9fa0a0"});
					$(".search .inputText").css({
						"color": "#6ba9c2"
					});
					$("#nav .search .inputText").css({
						"color": "#5d5d5d"
					});
				}
			});
			$(document).click(function(e) {
				var $objTarget = $(e.target);
				var $objTargetParent = $objTarget.parents();
				if(!$objTargetParent.is("#aotoCompeleteDivId2") && !$objTarget.is("#aotoCompeleteDivId2") && !$objTarget.is("#publicSearch")){
					$('#aotoCompeleteDivId2').hide();
				}
				if(!$objTargetParent.is("#aotoCompeleteDivId") && !$objTarget.is("#aotoCompeleteDivId") && !$objTarget.is("#searchHQ")){
					$('#aotoCompeleteDivId').hide();
				}
				if ($objTargetParent.is(".centerblock") || $objTarget.is(".centerblock")) {
				} else {
					$('#more_con').hide();
					$('.markets').show();
					$("#main .centerblock .ads").hide();
					$('#searchOpinion').val('股票名称或代码').css({"color": "#9fa0a0"});
					$(".search .inputText").css({
						"color": "#6ba9c2"
					});
					$("#nav .search .inputText").css({
						"color": "#5d5d5d"
					});
				}
			});
		}		
	};
}();
