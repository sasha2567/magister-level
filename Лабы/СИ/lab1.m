close all
clear all
clc
numberOfStarts = 4;

k(1)=0;
sim('first');
step = round(length(errors.signals.values) / numberOfStarts);
for i = 1 : numberOfStarts;
   errorsData(:, i) = errors.signals.values((((i - 1) * step) + 1) : (i * step));
   k(i + 1) = errorsData(end, i);
   errorsData(:, i) = errorsData(:, i) - k(i);
end

figure;
stem(1 : numberOfStarts, double(errorsData(end, :)) / (200 / numberOfStarts), 'k');
title('Распределение вероятностей отказа по функциональным разрезам');
xlabel('N разреза');
ylabel('P отк j');
grid on;

figure;

e = double(errorsData(end, :)) / length(errors.signals.values);

N = size(errorsData, 1) * size(errorsData, 2);
n = errorsData(end, :);
n0 = N - sum(n);
s = 0;
for i = 1 : numberOfStarts 
    c(i) = 0.4 + (mod(i, 21) / 100);
    s = s + c(i) * (n(i) - 1);
end

Rn = (1 / double(N)) * double(n0 + s);

R = cumprod(1 - e);
stem(1 : numberOfStarts, R, 'MarkerFaceColor', 'black', 'Marker', 'diamond');
title('Оценки вероятностей безотказной работы');
xlabel('N разреза');
ylabel('P');
grid on;

figure;
axis('off');
text(0, 1, sprintf('Число функциональных разрезов %d', numberOfStarts), 'FontSize', 10);
text(0, 0.9, sprintf('Число испытаний %d', N), 'FontSize', 10);
text(0, 0.8, sprintf('Оценка Коркорена %0.3f', Rn), 'FontSize', 10);
text(0, 0.7, sprintf('Оценка надёжости Нельсона %0.3f', R(end)), 'FontSize', 10);
