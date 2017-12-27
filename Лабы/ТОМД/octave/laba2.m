clear;
clc;
close all;
secs=5;
fsample = [300, 140];
for i = 1 : length(fsample)
  f1=25;
  f2=35;
  f3=75;
  t=(0:1/fsample(i):secs);
  N=length(t);
  a=sin(2*pi*f1*t);
  b=sin(2*pi*f2*t);
  c=sin(2*pi*f3*t);

  f = 0 : fsample(i)/ N : fsample(i);
  xa=(2/N)*abs(fft(a));
  xb=(2/N)*abs(fft(b));
  xc=(2/N)*abs(fft(c));
  
  d=a(1:N)+0.75*b(1:N)+0.5*c(1:N);
  spec_d=(2/N)*abs(fft(d));
  
  w = bartlett(N)';
  dw = d.*w;
  spec_dw = (2/N)*abs(fft(dw));
  
  wp = and(t > secs / 3, t < 2 * secs / 3);
  dwp = d .* wp;
  spec_dwp = (2/N)*abs(fft(dwp));

  figure;
  plot(xa);
  title('spectr a')
  xlabel('freq');
  ylabel('amp');
  
  figure;
  plot(xb);
  title('spectr b')
  xlabel('freq');
  ylabel('amp');
  
  figure;
  plot(xc);
  title('spectr c')
  xlabel('freq');
  ylabel('amp');

  figure;
  plot(d);
  title('summ a b c');
  xlabel('time');
  ylabel('amp');
  
  figure;
  plot(spec_d);
  title('spectr of summ');
  xlabel('freq');
  ylabel('amp');
  
  figure;
  plot(dw);
  title('sign * window bartlett');
  xlabel('time');
  ylabel('amp');
  
  figure;
  plot(dwp);
  title('sign * windowp');
  xlabel('time');
  ylabel('amp');
  
  figure;
  plot(spec_dw);
  title('spectr of sign * window bartlett');
  xlabel('time');
  ylabel('amp');
  
  figure;
  plot(spec_dwp);
  title('spectr of sign * windowp');
  xlabel('time');
  ylabel('amp');
end;