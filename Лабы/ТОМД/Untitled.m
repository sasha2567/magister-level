close all;
clear all;
order = 10;
g = 0.01;
 
filename = 'ee3.wav';
 
[y, fs] = audioread(filename);
y = y';
 
k1 = 441;
k2 = 64;
 
y_long = y(1 : k1);
y_short = y(1 : k2);
f_long = 0 : fs / length(y_long) : fs / 2 - fs / length(y_long);
f_short = 0 : fs / length(y_short) : fs / 2 - fs / length(y_short);
s_long = abs(fft(y_long));
s_short = abs(fft(y_short));
ls_long = log(s_long);
ls_short = log(s_short);
figure;
plot(f_long, ls_long(1 : length(ls_long) / 2));
grid on;
title('Narrow band log frequency responce');
xlabel('Log F');
ylabel('Amplitude');
figure;
plot(f_short, ls_short(1 : length(ls_short) / 2));
grid on;
title('Wide band log frequency responce');
xlabel('Log F');
ylabel('Amplitude');
 
s_y = fft(y);
a_s_y = abs(s_y);
log_as_y = log(abs(s_y));
c_y = ifft(log_as_y);
w = 100;
c_w = c_y;
c_w(w : (length(c_y) - w)) = 0;
 
s_c_y = fft(c_y);
s_c_w = fft(c_w);
 
f = 0 : fs / length(y) : fs / 2 - fs / length(y);
figure
plot(f, s_c_w(1 : length(s_c_w) / 2));
grid on;
title('Log frequency response');
xlabel('Log F');
ylabel('Amplitude');
 
a = lpc(y, order);
y_filtered = filter(g, a, y);
[h, w] = freqz(g, a);
figure;
plot(w * fs / pi / 2, log(abs(h))');
grid on;
title('Log filter frequency response');
xlabel('Log F');
ylabel('Amplitude');
 
figure;
plot(f_long, ls_long(1 : length(ls_long) / 2), 'b');
hold on;
plot(f_short, ls_short(1 : length(ls_short) / 2), 'r');
plot(f, s_c_w(1 : length(s_c_w) / 2), 'g');
plot(w * fs / pi / 2, log(abs(h)), 'y');
grid on;
title('Log frequency response');
xlabel('Log F');
ylabel('Amplitude');
legend('Narrow band frequency response', 'Wide band frequency response', 'Circumflex of frequency responce', 'Filter frequency response');
 
t = 0 : 1 / fs : (length(y) - 1) / fs;
 
figure;
plot(t, y, 'b');
hold on;
plot(t, y_filtered, 'g');
grid on;
title('Real and predicted signals');
xlabel('Time, sec');
ylabel('Amplitude');
legend('Real signal', 'Predicted signal')
 
e = y - y_filtered;
 
figure;
plot(t, e);
grid on;
title('Difference between real and predicted signals');
xlabel('Time, sec');
ylabel('Amplitude');
