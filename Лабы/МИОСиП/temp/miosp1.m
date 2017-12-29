clc;
close all;
clear all;
sim('miosp');

x = process;
for rr = 1: 100
    ff(rr) = 1/pi/(rr^2+1);
end;
figure;
plot(ff);
n = 10 : 10 : 50;
h = zeros(1, 5);
s = zeros(1, 5);
q = zeros(1, 5);
i = zeros(1, 5);

for j = 1 : 5
    [a, b, c, d] = compute_enthropy(x, n(j));
    h(j) = a
    s(j) = b
    q(j) = c
    i(j) = d
end

figure;
scatter(q ./ s, h);

figure;
scatter(q ./ s, i);
X0 = process;
char(13)
q = 0.2;
a = 0.9;
s = 1;

for i = 1 : 3
    X0 = X0 - mean(X0);
    X0centr_norm = (X0 - mean(X0)) / std(X0);
    X0big = X0 / q;
    X0round = round(X0big);
    X0quant = X0round * q;
    X0nois = X0 - X0quant;
    figure;
    grid on;
    subplot(3, 1, 1);
    plot(X0(1 : 100));
    subplot(3, 1, 2);
    plot(X0quant(1 : 100));
    subplot(3, 1, 3);
    plot(X0nois(1 : 100));
    figure;
    subplot(3, 1, 1);
    pwelch(X0, [], [], [], 1);
    subplot(3, 1, 2);
    pwelch(X0quant, [], [], [], 1);
    subplot(3, 1, 3);
    pwelch(X0nois, [], [], [], 1);
    mean(X0)
    std(X0)
    var(X0nois)
    s = std(X0);
    q2 = (max(X0) - min(X0));
    i2 = (1 / 2) * log2(1 + (12 * s) / (q2 ^ 2))

    im = i2 / q
    X0 = filter(s * sqrt(1 - a ^ 2), [1, -a], X0);
end
