/*
 * jQuery SimpleCalculadora
 * @author dimti28@gmail.com - http://develoteca.com
 * @version 1.0
 * @date Julio 10, 2015
 * @category jQuery plugin
 * @copyright (c) 2015 dimti28@gmail.com (http://develoteca.com)
 * @license CC Attribution-NonCommercial-ShareAlike 3.0 Unported (CC BY-NC-SA 3.0) - http://creativecommons.org/licenses/by-nc-sa/3.0/
 */

jQuery.fn.extend({
    Calculadora: function (op) {
        var LaCalculadora = this;
        var idInstancia = $(LaCalculadora).attr('id');
        var NombreBotonesClase = idInstancia + 'tcl';
        var Clase;
        var Botones;
        var Signos;

        defaults = {
            TituloHTML: '',
            Botones: ["7", "8", "9", "+", "4", "5", "6", "-", "3", "2", "1", "*", "0", ".", "=", "/"],
            Signos: ["+", "-", "*", "/"],
            ClaseBtns1: 'primary',
            ClaseBtns2: 'success',
            ClaseBtns3: 'warning',
            ClaseColumnas: 'col-md-3 mbottom',
            ClaseBotones: 'btn3d btn-lg btn-block btn btn-',
            txtSalida: idInstancia + 'txtResultado',
            ClasetxtSalida: 'form-control txtr',
            InputBorrar: idInstancia + 'Borrar',
            ClaseInputBorrar: 'btn3d btn btn-danger btn-lg btn-block',
            EtiquetaBorrar: 'Borrar'
        }

        var op = $.extend({}, defaults, op);
        Botones = op.Botones;
        Signos = op.Signos;
        $(LaCalculadora).append('<input type="text" class="' + op.ClasetxtSalida + '" id="' + op.txtSalida + '" value="0" >');
        $(LaCalculadora).append('<div class="row" id="' + idInstancia + 'btns"></div>');
        $.each(Botones, function (index, value) {
            Clase = op.ClaseBtns1
            if (Signos.indexOf(value) > -1) { Clase = op.ClaseBtns2; }
            if (value == '=') { Clase = op.ClaseBtns3; }
            $('#' + idInstancia + 'btns').append('<div class="' + op.ClaseColumnas + '"><input type="button" class="' + NombreBotonesClase + ' ' + op.ClaseBotones + Clase + '" value="' + value + '"/></div>');
        });
        $(LaCalculadora).append('<input type="button" id="' + op.InputBorrar + '" class="' + op.ClaseInputBorrar + '" value="' + op.EtiquetaBorrar + '">');
        $(LaCalculadora).html('<div class="panel panel-primary btn-block calculadoraBase  mtop">' + op.TituloHTML + '<div class="panel-body"><div class="col-md-12" style="margin-bottom: 10px;">' + $(LaCalculadora).html() + '</div></div> </div>');

        $('.' + NombreBotonesClase).click(function () {
            var vTecla = $(this).val();
            var salida = $('#' + op.txtSalida);
            if (vTecla == '=') { salida.val(eval(salida.val())); }
            else {
                if ((salida.val() == 0)) {
                    if (Signos.indexOf(vTecla) > -1) { salida.val(0) }
                    else { salida.val(vTecla); }
                } else { salida.val(salida.val() + vTecla); }
            }
        });
        $("#" + op.InputBorrar).click(function () { $('#' + op.txtSalida).val("0"); });
    }
});

//贷款计算器

function $id2(id) {
    return document.getElementById(id);
};
function C(tap, parentNode) {
    var obj = document.createElement(tap);
    parentNode.appendChild(obj);
    return obj;
};
function Compute() {
    var I = this;
    I.txtMoney1 = $id2('txtMoney1');
    I.txtMoney2 = $id2('txtMoney2');
    I.txtMonth1 = $id2('txtMonth1');
    I.txtMonth2 = $id2('txtMonth2');
    I.txtAPR1 = $id2('txtAPR1');
    I.txtAPR2 = $id2('txtAPR2');
    I.txtType1 = $id2('txtType1');
    I.btnCompute = $id2('btnCompute');
    I.divMain = $id2('divMain');

    //I.divCreater = $('divCreater');
    //I.divCreater.style.position = 'absolute';
    //I.divCreater.style.top = 30;

    //I.divCreater.style.left = document.body.clientWidth - I.divCreater.offsetWidth - 50;

    I.btnCompute.onclick = function () {
        I.Print();
    };
};
Compute.prototype.Print = function () {
    var I = this;
    I.divMain.style.display = 'block';
    I.divMain.innerHTML = '';
    I.table = C('table', I.divMain); I.table.className = 'table table-hover table-striped table-bordered table-condensed';
    I.thead = C('thead', I.table);
    I.tfoot = C('tfoot', I.table);
    var tr = C('tr', I.thead);
    var th = C('th', tr); th.innerHTML = '期数'; th.rowSpan = '2';
    th = C('th', tr); th.innerHTML = '本期应还总额'; th.colSpan = '3';
    th = C('th', tr); th.innerHTML = '本期应还本金'; th.colSpan = '3';
    th = C('th', tr); th.innerHTML = '本期应还利息'; th.colSpan = '3';
    th = C('th', tr); th.innerHTML = '剩余本金'; th.colSpan = '3';

    tr = C('tr', I.thead);
    th = C('th', tr); th.innerHTML = '商业';
    th = C('th', tr); th.innerHTML = '公积金';
    th = C('th', tr); th.innerHTML = '合计';
    th = C('th', tr); th.innerHTML = '商业';
    th = C('th', tr); th.innerHTML = '公积金';
    th = C('th', tr); th.innerHTML = '合计';
    th = C('th', tr); th.innerHTML = '商业';
    th = C('th', tr); th.innerHTML = '公积金';
    th = C('th', tr); th.innerHTML = '合计';
    th = C('th', tr); th.innerHTML = '商业';
    th = C('th', tr); th.innerHTML = '公积金';
    th = C('th', tr); th.innerHTML = '合计';

    I.tbody = C('tbody', I.table);

    var money1 = parseFloat(I.txtMoney1.value);
    var money2 = parseFloat(I.txtMoney2.value);
    var month1 = parseInt(I.txtMonth1.value);
    var month2 = parseInt(I.txtMonth2.value);
    var apr1 = parseFloat(I.txtAPR1.value) / 100;
    var apr2 = parseFloat(I.txtAPR2.value) / 100;

    if (I.txtType1.value == '1') {
        I.PrintType1(money1, month1, apr1, money2, month2, apr2);
    }
    else if (I.txtType1.value == '2') {
        I.PrintType2(money1, month1, apr1, money2, month2, apr2);
    }

};
Compute.prototype.PrintType1 = function (money1, month1, apr1, money2, month2, apr2)//等额本息
{
    var I = this;

    var mapr1 = apr1 / 12; //商业月利率
    var mapr2 = apr2 / 12; //公积金月利率

    /*
    //1个月
    money * (1 + mapr) - x = 0;
    //2个月
    (money * (1 + mapr) - x) * (1 + mapr) - x = 0;
    //3个月
    ((money * (1 + mapr) - x) * (1 + mapr) - x)) * (1 + mapr) - x = 0;
    //4个月
    (((money * (1 + mapr) - x) * (1 + mapr) - x)) * (1 + mapr) - x)) * (1 + mapr) - x = 0;

    money * (1 + mapr)^month = x(1 + (1 + mapr) + (1 + mapr)^2 + (1 + mapr)^3 + ... + (1 + mapr)^(month-1))



                        money * (1 + mapr) ^ month
    x = -------------------------------------------------------------------------
    1 + (1 + mapr) + (1 + mapr)^2 + ... + (1 + mapr)^(month - 1)

    */

    var fm1 = 1;
    for (var i = 1; i < month1; i++) {
        fm1 += Math.pow(1 + mapr1, i);
    }

    var fm2 = 1;
    for (var i = 1; i < month2; i++) {
        fm2 += Math.pow(1 + mapr2, i);
    }

    var perMonth1 = (money1 * Math.pow(1 + mapr1, month1) / fm1).toFixed(2); //商业每月还款额,保留两位小数
    var perMonth2 = (money2 * Math.pow(1 + mapr2, month2) / fm2).toFixed(2); //公积金每月还款额,保留两位小数

    var tr, td, leftMoney1 = money1, leftMoney2 = money2;
    var month = (month1 > month2 ? month1 : month2);
    var totalPerMonth1 = 0, totalPerMonth2 = 0, totalPerMonth = 0, totalBJ1 = 0, totalBJ2 = 0, totalBJ = 0, totalLX1 = 0, totalLX2 = 0, totalLX = 0;
    for (var i = 1; i <= month; i++) {
        var lx1 = (leftMoney1 * mapr1).toFixed(2);
        leftMoney1 = (leftMoney1 * (1 + mapr1) - perMonth1).toFixed(2);

        var lx2 = (leftMoney2 * mapr2).toFixed(2);
        leftMoney2 = (leftMoney2 * (1 + mapr2) - perMonth2).toFixed(2);

        tr = C('tr', I.tbody);
        td = C('td', tr); td.innerHTML = i; //还款期数
        if (i <= month1) {
            td = C('td', tr); td.innerHTML = perMonth1; //商业还款额
            totalPerMonth1 += parseFloat(perMonth1);
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month2) {
            td = C('td', tr); td.innerHTML = perMonth2; //公积金还款额
            totalPerMonth2 += parseFloat(perMonth2);
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month1 && i <= month2) {
            td = C('td', tr); td.innerHTML = (parseFloat(perMonth1) + parseFloat(perMonth2)).toFixed(2); //还款额合计
            totalPerMonth += parseFloat(perMonth1) + parseFloat(perMonth2);
        }
        else if (i <= month1) {
            td = C('td', tr); td.innerHTML = perMonth1;
            totalPerMonth += parseFloat(perMonth1);
        }
        else if (i <= month2) {
            td = C('td', tr); td.innerHTML = perMonth2;
            totalPerMonth += parseFloat(perMonth2);
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month1) {
            td = C('td', tr); td.innerHTML = (perMonth1 - lx1).toFixed(2); //商业本期应还本金
            totalBJ1 += perMonth1 - lx1;
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month2) {
            td = C('td', tr); td.innerHTML = (perMonth2 - lx2).toFixed(2); //公积金本期应还本金
            totalBJ2 += perMonth2 - lx2;
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month1 && i <= month2) {
            td = C('td', tr); td.innerHTML = (parseFloat(perMonth1) - parseFloat(lx1) + parseFloat(perMonth2) - parseFloat(lx2)).toFixed(2); //本期应还本金合计
            totalBJ += parseFloat(perMonth1) - parseFloat(lx1) + parseFloat(perMonth2) - parseFloat(lx2);
        }
        else if (i <= month1) {
            td = C('td', tr); td.innerHTML = (perMonth1 - lx1).toFixed(2);
            totalBJ += parseFloat(perMonth1 - lx1);
        }
        else if (i <= month2) {
            td = C('td', tr); td.innerHTML = (perMonth2 - lx2).toFixed(2);
            totalBJ += parseFloat(perMonth2 - lx2);
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month1) {
            td = C('td', tr); td.innerHTML = lx1; //商业本期应还利息
            totalLX1 += parseFloat(lx1);
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month2) {
            td = C('td', tr); td.innerHTML = lx2; //公积金本期应还利息
            totalLX2 += parseFloat(lx2);
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month1 && i <= month2) {
            td = C('td', tr); td.innerHTML = (parseFloat(lx1) + parseFloat(lx2)).toFixed(2); //公积金本期应还利息
            totalLX += parseFloat(lx1) + parseFloat(lx2);
        }
        else if (i <= month1) {
            td = C('td', tr); td.innerHTML = lx1;
            totalLX += parseFloat(lx1);
        }
        else if (i <= month2) {
            td = C('td', tr); td.innerHTML = lx2;
            totalLX += parseFloat(lx2);
        }

        if (i <= month1) {
            td = C('td', tr); td.innerHTML = leftMoney1; //商业剩余本金
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month2) {
            td = C('td', tr); td.innerHTML = leftMoney2; //公积金剩余本金
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month1 && i <= month2) {
            td = C('td', tr); td.innerHTML = (parseFloat(leftMoney1) + parseFloat(leftMoney2)).toFixed(2); //剩余本金合计
        }
        else if (i <= month1) {
            td = C('td', tr); td.innerHTML = leftMoney1;
        }
        else if (i <= month2) {
            td = C('td', tr); td.innerHTML = leftMoney2;
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }
    }
    tr = C('tr', I.tfoot);
    td = C('td', tr); td.innerHTML = '合计：';
    td = C('td', tr); td.innerHTML = totalPerMonth1.toFixed(2);
    td = C('td', tr); td.innerHTML = totalPerMonth2.toFixed(2);
    td = C('td', tr); td.innerHTML = totalPerMonth.toFixed(2);
    td = C('td', tr); td.innerHTML = totalBJ1.toFixed(2);
    td = C('td', tr); td.innerHTML = totalBJ2.toFixed(2);
    td = C('td', tr); td.innerHTML = totalBJ.toFixed(2);

    td = C('td', tr); td.innerHTML = totalLX1.toFixed(2);
    td = C('td', tr); td.innerHTML = totalLX2.toFixed(2);
    td = C('td', tr); td.innerHTML = totalLX.toFixed(2);

};
Compute.prototype.PrintType2 = function (money1, month1, apr1, money2, month2, apr2)//等额本金
{
    var I = this;

    var mapr1 = apr1 / 12; //商业月利率
    var mapr2 = apr2 / 12; //公积金月利率

    //每月还本金
    var bj1 = money1 / month1;

    //每月还本金
    var bj2 = money2 / month2;

    var tr, td, leftMoney1 = money1, leftMoney2 = money2;

    var month = (month1 > month2 ? month1 : month2);
    var totalPerMonth1 = 0, totalPerMonth2 = 0, totalPerMonth = 0, totalBJ1 = 0, totalBJ2 = 0, totalBJ = 0, totalLX1 = 0, totalLX2 = 0, totalLX = 0;
    for (var i = 1; i <= month; i++) {
        var lx1 = (leftMoney1 * mapr1).toFixed(2);
        var perMoney1 = (parseFloat(bj1) + parseFloat(lx1)).toFixed(2); //本期还款
        leftMoney1 = (leftMoney1 * (1 + mapr1) - perMoney1).toFixed(2);

        var lx2 = (leftMoney2 * mapr2).toFixed(2);
        var perMoney2 = (parseFloat(bj2) + parseFloat(lx2)).toFixed(2); //本期还款
        leftMoney2 = (leftMoney2 * (1 + mapr2) - perMoney2).toFixed(2);

        tr = C('tr', I.tbody);
        td = C('td', tr); td.innerHTML = i; //还款期数

        if (i <= month1) {
            td = C('td', tr); td.innerHTML = perMoney1; //还款额
            totalPerMonth1 += parseFloat(perMoney1);
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month2) {
            td = C('td', tr); td.innerHTML = perMoney2; //还款额
            totalPerMonth2 += parseFloat(perMoney2);
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month1 && i <= month2) {
            td = C('td', tr); td.innerHTML = (parseFloat(perMoney1) + parseFloat(perMoney2)).toFixed(2); //还款额
            totalPerMonth += parseFloat(perMoney1) + parseFloat(perMoney2);
        }
        else if (i <= month1) {
            td = C('td', tr); td.innerHTML = perMoney1;
            totalPerMonth += parseFloat(perMoney1);
        }
        else if (i <= month2) {
            td = C('td', tr); td.innerHTML = perMoney2;
            totalPerMonth += parseFloat(perMoney2);
        }

        if (i <= month1) {
            td = C('td', tr); td.innerHTML = bj1.toFixed(2); //本期应还本金
            totalBJ1 += bj1;
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month2) {
            td = C('td', tr); td.innerHTML = bj2.toFixed(2); //本期应还本金
            totalBJ2 += bj2;
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month1 && i <= month2) {
            td = C('td', tr); td.innerHTML = (parseFloat(bj1) + parseFloat(bj2)).toFixed(2); //本期应还本金
            totalBJ += parseFloat(bj1) + parseFloat(bj2);
        }
        else if (i <= month1) {
            td = C('td', tr); td.innerHTML = bj1;
            totalBJ += parseFloat(bj1);
        }
        else if (i <= month2) {
            td = C('td', tr); td.innerHTML = bj2;
            totalBJ += parseFloat(bj2);
        }

        if (i <= month1) {
            td = C('td', tr); td.innerHTML = lx1; //本期应还利息
            totalLX1 += parseFloat(lx1);
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month2) {
            td = C('td', tr); td.innerHTML = lx2; //本期应还利息
            totalLX2 += parseFloat(lx2);
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month1 && i <= month2) {
            td = C('td', tr); td.innerHTML = (parseFloat(lx1) + parseFloat(lx2)).toFixed(2); //本期应还利息
            totalLX += parseFloat(lx1) + parseFloat(lx2);
        }
        else if (i <= month1) {
            td = C('td', tr); td.innerHTML = lx1;
            totalLX += parseFloat(lx1);
        }
        else if (i <= month2) {
            td = C('td', tr); td.innerHTML = lx2;
            totalLX += parseFloat(lx2);
        }

        if (i <= month1) {
            td = C('td', tr); td.innerHTML = leftMoney1; //剩余本金
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month2) {
            td = C('td', tr); td.innerHTML = leftMoney2; //剩余本金
        }
        else {
            td = C('td', tr); td.innerHTML = '';
        }

        if (i <= month1 && i <= month2) {
            td = C('td', tr); td.innerHTML = (parseFloat(leftMoney1) + parseFloat(leftMoney2)).toFixed(2); //本期应还利息
        }
        else if (i <= month1) {
            td = C('td', tr); td.innerHTML = leftMoney1;
        }
        else if (i <= month2) {
            td = C('td', tr); td.innerHTML = leftMoney2;
        }
    }

    tr = C('tr', I.tfoot);
    td = C('td', tr); td.innerHTML = '合计：';
    td = C('td', tr); td.innerHTML = totalPerMonth1.toFixed(2);
    td = C('td', tr); td.innerHTML = totalPerMonth2.toFixed(2);
    td = C('td', tr); td.innerHTML = totalPerMonth.toFixed(2);
    td = C('td', tr); td.innerHTML = totalBJ1.toFixed(2);
    td = C('td', tr); td.innerHTML = totalBJ2.toFixed(2);
    td = C('td', tr); td.innerHTML = totalBJ.toFixed(2);

    td = C('td', tr); td.innerHTML = totalLX1.toFixed(2);
    td = C('td', tr); td.innerHTML = totalLX2.toFixed(2);
    td = C('td', tr); td.innerHTML = totalLX.toFixed(2);
};

