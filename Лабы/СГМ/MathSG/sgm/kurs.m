% Основной файл
% менять параметры по варианту в файле generateProcess.m
% и, возможно, ниже
clear all
close all
clc

[y_sum, t_model] = generateProcess;
y_sum2 = generateProcess;

modelSize = size(t_model);
n = modelSize(2);

%вычисление математического ожидания и среднеквадратического отклонения
m1 = mean(y_sum)
sigma1 = var(y_sum)

m2 = mean(y_sum2)
sigma2 = var(y_sum2)

%выборочные АКФ и ЧАКФ
acf = crosscorr(y_sum, y_sum, (n - 1));
figure
plot(acf)

pacf = parcorr(y_sum);
%parcorr(y_sum, (n - 1));
figure
plot(pacf)

%Расчёт параметра d (переменная i)
i = 0;
y_integr = y_sum;
while not(adftest(y_integr))%тест Дики-Фулера
    y_integr = diff(y_integr);
    i = i + 1;
end

%выборочные АКФ и ЧАКФ интегрированного процесса

acfi = crosscorr(y_integr, y_integr, (n - i - 1));
figure
plot(acfi)

pacfi = parcorr(y_integr);
figure
plot(pacfi)

%По графикам выборочного АКФ и ЧАКФ надо найти параметры p и q
%как их подберать написано в начале с. 10 новой методички
%У меня получилось p == q != 0, и я рассматривал 2 варианта

%p == q == 1

p = 1;
q = 1;

[y_pred, residuals] = arimaPrediction(p, i, q, y_sum);

%проверка на адекватность -- автокорреляция остатков
p1q1cov = cov(residuals)

%и p == q == 2

p=2;
q=2;

[y_pred, residuals] = arimaPrediction(p, i, q, y_sum);

%проверка на адекватность -- автокорреляция остатков
p2q2cov = cov(residuals)

% Y -- матрица, в которой i-я строка -- временной ряд.
% Типа:
%   / 0.16 0.21 0.19 0.25 ... \
%   \ 0.11 0.09 0.14 0.11 ... /
Y = [y_sum; y_sum2];

%векторная авторегрессия
vectorAutoregressionPrediction(t_model, Y);

%Построения графика 2-х рядов матрицы Y
figure
plot(t_model,Y(1,:),t_model,Y(2,:))

%Тест Энгла-Грейнджера (проверка на коинтеграцию)
cointegration = egcitest(Y')

