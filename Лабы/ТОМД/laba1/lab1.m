clear, clc, close all

A = 1;
T = 2; % период сигналов
tau = 1; % длительность импульса
N = 20; % количество оставленных гармоник
dt = T/N/50; % шаг по времени
t = -T/2:dt:T/2; % моменты времени
k = 1:N;
x1 = zeros(size(t));
x1( abs(t) <= tau/2 ) = 1;
x1( abs(t) >= tau/2 ) = -1;

% коэффициенты разложения чётной функции
a0 = 0;
ak = 4*(sin(pi*k/2)-sin(pi*k))/pi./k;

% вычисление суммы
s1(1:length(t)) = a0/2;
for n = 1 : N
 s1 = s1 + ak(n)*cos(pi*n*t);
end

% сравниваем на графиках исходные и приближенные функции
plot(t,x1,'k', t,s1,'k:')
grid on

e = x1 - s1;
d = norm(e) / norm(x1)

figure;
stem(abs(ak));