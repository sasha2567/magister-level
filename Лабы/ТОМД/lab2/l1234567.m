clear;
clc;
close all;
seconds=5;
fsample=140;
f1=25;
f2=35;
f3=75;
t=(0:1/fsample:seconds);
N=length(t);
a=sin(2*pi*f1*t);
b=sin(2*pi*f2*t);
c=sin(2*pi*f3*t);

f = 0 : fsample/ N : fsample1;
xa=abs(fft(a));
xb=abs(fft(b));
xc=abs(fft(c));

figure;
plot(xa)
figure;
plot(xb)
figure;
plot(xc)

d=a(1:N)+0.75*b(1:N)+0.5*c(1:N);
spec=(1/(N/2))*abs(fft(d));

figure;
plot(spec)

w = bartlett(N)';
dw = d.*w;

figure;
plot(d)
figure;
plot(dw)
figure;
plot(abs(fft(d)))
figure;
plot(abs(fft(dw)))