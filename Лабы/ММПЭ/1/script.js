var X = [];
var Y = [];
var Acoef = 0;
var Bcoef = 0;
var Ccoef = 0;
var mode = 'linear';
$(document).ready(function () {
  $('.add-asix').click(function (e) {
    e.stopProrrogation;
    var x = $('.x-asix').val();
    var y = $('.y-asix').val();
    $('.asix-data').append('<tr><td class="x-data">' + x + '</td><td class="y-data">' + y + '</td></tr>');
    $('.x-asix').val('');
    $('.y-asix').val('');
    X.push(x);
    Y.push(y);
  });
  $('.type').change(function (e) {
    e.stopProrrogation;
    mode = $(this).val();
  })

  var determenant2 = function (matrix) {
    return  matrix[0][0]*matrix[1][1] -
            matrix[0][1]*matrix[1][0];
  }

  var determenant3 = function (matrix) {
    return  matrix[0][0]*matrix[1][1]*matrix[2][2] +
            matrix[0][1]*matrix[1][2]*matrix[2][0] +
            matrix[0][2]*matrix[1][0]*matrix[2][1] -
            
            matrix[0][2]*matrix[1][1]*matrix[2][0] -
            matrix[0][0]*matrix[1][2]*matrix[2][1] -
            matrix[0][1]*matrix[1][0]*matrix[2][2];
  }

  var aprox2 = function () {
    var n = X.length;
    var A = 0;
    var B = 0;
    var C = 0;
    var D = 0;
    for (var i = X.length - 1; i >= 0; i--) {
      A += parseInt(X[i]);
      B += parseInt(Y[i]);
      C += X[i]*X[i];
      D += X[i]*Y[i];
    }
    var tmp = [[n, A], [A, C]];
    var a = determenant2(tmp);
    tmp = [[B, A], [D, C]];
    Acoef = determenant2(tmp)/a;
    tmp = [[n, B], [A, D]];
    Bcoef = determenant2(tmp)/a;
    console.log('Acoef = ' + Acoef);
    console.log('Bcoef = ' + Bcoef);
    return [Acoef, Bcoef];
  }

  var aprox3 = function () {
    var n = X.length;
    var A = 0;
    var B = 0;
    var C = 0;
    var D = 0;
    var E = 0;
    var F = 0;
    var G = 0;
    for (var i = X.length - 1; i >= 0; i--) {
      A += parseInt(X[i]);
      B += parseInt(Y[i]);
      C += X[i]*X[i];
      D += X[i]*Y[i];
      E += X[i]*X[i]*X[i];
      F += X[i]*X[i]*X[i]*X[i];
      G += X[i]*X[i]*Y[i];
    }
    var tmp = [[n, A, C], [A, C, E], [C, E, F]];
    var a = determenant3(tmp);
    tmp = [[B, A, C], [D, C, E], [G, E, F]];
    Acoef = determenant3(tmp)/a;
    tmp = [[n, B, C], [A, D, E], [C, G, F]];
    Bcoef = determenant3(tmp)/a;
    tmp = [[n, A, B], [A, C, D], [C, E, G]];
    Ccoef = determenant3(tmp)/a;
    console.log('Acoef = ' + Acoef);
    console.log('Bcoef = ' + Bcoef);
    console.log('Ccoef = ' + Ccoef);
    return [Acoef, Bcoef, Ccoef];
  }

  $('.aprox').click(function (e) {
    e.stopProrrogation;
    if (mode == 'linear'){
      plotCreateGrefics(dataFormation(aprox2()));
      epsilant();
    }
    else {
      plotCreateGrefics(dataFormation(aprox3())); 
      epsilant();
    }
  });

  var dataFormation = function (argument) {
    var data = new Array();
    data[0] = new Object();
    data[1] = new Object();
    data[0].label = "Исходный ряд";
    data[0].color = 2;
    data[1].label = "Апроксимация";
    data[1].color = 3;
    var tmp = new Array();
    for (var i = X.length - 1; i >= 0; i--) {
      tmp.push(['200' + X[i]+'/01/01', Y[i]]);
    }
    data[0].data = tmp;
    if (argument.length > 2) {  
      tmp = new Array();
      for (var i = X.length - 1; i >= 0; i--) {
        tmp.push(['200' + X[i]+'/01/01', argument[0] + argument[1]*X[i] + argument[2]*X[i]*X[i]]);
      }
      data[1].data = tmp;
    }
    else {
      tmp = new Array();
      for (var i = X.length - 1; i >= 0; i--) {
        tmp.push(['200' + X[i]+'/01/01', argument[0] + argument[1]*X[i]]);
      }
      data[1].data = tmp;
    }
    return data;
  }

  var epsilant = function () {
    var sum = 0;
    for (var i = X.length - 1; i >= 0; i--) {
      if(mode == 'linear'){
        sum += (Y[i] - Bcoef * X[i] - Acoef)*(Y[i] - Bcoef * X[i] - Acoef);
      }
      else{
        sum += (Y[i] - Ccoef * X[i] * X[i] - Bcoef * X[i] - Acoef)*(Y[i] - Ccoef * X[i] * X[i] - Bcoef * X[i] - Acoef);
      }
    }
    console.log(sum);
  }

  var plotCreateGrefics = function (data) {
    // выделенная область
    var selection = ["200" + Math.min.apply(null, X)+'/01/01', "200" + Math.max.apply(null, X)+'/01/01'];
    // все данные
    console.log(selection);
    // цвета задавать обязательно, иначе они будут все время меняться при удалении/добавлении рядов
    var all_data = data;

    // какие данные скрываем - заполняем позже
    var hide = [];
    // преобразуем даты в формат, понятный Flot'у
    for(var j = 0; j < all_data.length; ++j) {
      hide.push(false); // не скрываем j-ый ряд. пока что.
      for(var i = 0; i < all_data[j].data.length; ++i)
        all_data[j].data[i][0] = Date.parse(all_data[j].data[i][0]);
    }
    for(var i = 0; i < selection.length; ++i)
      selection[i] = Date.parse(selection[i]);

    var plot; // график крупным планом
    var show_bars = false; // показывать столбики или линии
    var plot_conf = {
      series: {
        stack: null,
        lines: { 
          show: true,
          lineWidth: 2 
        }
      },
      xaxis: {
        mode: "time",
        timeformat: "%y",
        min: selection[0],
        max: selection[1]
      },
      legend: {
        container: $("#legend")
      }
    };

    // меняем вид - столбики или линии
    function switch_show() {
      show_bars = !show_bars; // изменяем тип диаграм

      var new_conf = {
        series: {
          stack: show_bars ? true : null,
          lines: { show: !show_bars },
          bars: { show: show_bars }
        }
      };

      // обновляем конфиг
      $.extend(true, plot_conf, new_conf);

      // перерисовываем
      redraw();
    }

    // перерисовываем все и вся :)
    function redraw() {
      var data = [];
      for(var j = 0; j < all_data.length; ++j)
        if(!hide[j])
          data.push(all_data[j]);

      plot = $.plot($("#placeholder"), data, plot_conf);

      // легенду рисуем только один раз
      plot_conf.legend.show = false;

    }

    // вычисляем ширину колонки в соответствии с новой областью выделения
    function calc_bar_width() {
      // поскольку по оси OX откладывается время,
      // ширина столбцов в гистограмме вычисляется в 1/1000-ых секунды
      // при масштабировании эту величину следует пересчитать
      var r = plot_conf.xaxis;
      // вычисляем, сколько столбцов попало в интервал
      var bars_count = 0;
      for(var i = 0; i < all_data[0].data.length; ++i)
        if(all_data[0].data[i][0] >= r.min &&
           all_data[0].data[i][0] <= r.max)
           bars_count++;

      // изменяем ширину столбцов
      var new_conf = {
        series: {
          bars: { // умножаем на два, чтобы оставалось место между столбцами
            barWidth: (r.max - r.min)/((bars_count + 1 /* на ноль не делим */) * 2) 
          }
        }
      };
      $.extend(true, plot_conf, new_conf);
    }

    // вычисляем ширину столбцов в гистограмме
    calc_bar_width();
    // рисуем графики в первый раз
    redraw();

    // рисуем чекбоксы в легенде
    var legend = document.getElementById('legend'); // еще IE не умеет заменять innerHTML в table
    var legend_tbl = legend.getElementsByTagName('table')[0];
    var legend_html = '<table style="font-size: smaller; color: rgb(84, 84, 84);"><tbody>';
    for(var i = 0; i < legend_tbl.rows.length; i++) {
      legend_html += '<tr>' +
        '<td><input type="checkbox" onclick="hide['+ i +']=!hide['+ i +'];redraw();" checked="1"></td>'
        + legend_tbl.rows[i].innerHTML
        + '</tr>';
    }
    legend_html += "</tbody></table>";
    legend.innerHTML = legend_html;
  }  
})