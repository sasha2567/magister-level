clc;
close all;
clear all;

f1 = 2350;
f2 = 3550;
fsample1 = 14000;
fsample2 = 7000;
fsample3 = 42000;
secs = 0.5;
filter_order_1 = 20;
filter_order_2 = 100;
frequency_label = "Frequency, Hz";
time_label = "Time, s";
amplitude_label = "Amplitude";

t = 0 : 1 / fsample1 : secs - 1 / fsample1;
t2 = 0 : 1 / fsample2 : secs - 1 / fsample2;
t3 = 0 : 1 / fsample3 : secs - 1 / fsample3;
frequency_axis_1 = 0 : fsample1 / length(t) : fsample1 - fsample1 / length(t);
frequency_axis_2 = 0 : fsample1 / length(t) : fsample2 - fsample1 / length(t);
frequency_axis_3 = 0 : fsample1 / length(t) : fsample3 - fsample1 / length(t);

x = sin(2 * pi * f1 * t) + 0.75 * sin(2 * pi * f2 * t);
N = length(x);
as = (1 / (N / 2)) * abs(fft(x));
decimation = fsample1 / fsample2;
h_down = fir1(filter_order_1, 1 / (2 * decimation));
[h, w] = freqz(h_down, 1, filter_order_1);
x_filtered_down = filter(h_down, 1, x);
i = 1 : N / decimation;
y_down = x_filtered_down(i * decimation);
yw_down = y_down .* hanning(length(y_down))';
as_down = (1 / (N / 2)) * abs(fft(yw_down));
interpolation = fsample3 / fsample1;
y_up = zeros(1, N * interpolation);
i = 1 : N;
y_up(i * interpolation) = x(i);
as__up = (1 / (N / 2)) * abs(fft(y_up));
h_up = fir1(filter_order_2, 1 / (2 * interpolation));
[h1, w1] = freqz(h_up, 1, filter_order_2);
y_filtered_up = filter(h_up, 1, y_up);
yw_up = y_filtered_up .* hanning(length(y_up))';
as_up = (1 / (N / 2)) * abs(fft(yw_up));

figure;
plot(t, x);
title("Generated signal");
xlabel(time_label);
ylabel(amplitude_label);

figure;
plot(frequency_axis_1, as);
title("Spectrum of generated signal");
xlabel(frequency_label);
ylabel(amplitude_label);

figure;
plot(h_down);
title("Impulse responce of filter");

figure;
plot((w / max(w)) * fsample1, abs(h));
title("Frequency responce of filter");
xlabel(frequency_label);
ylabel(amplitude_label);

figure;
plot(t2, y_down);
title("Filtered signal");
xlabel(time_label);
ylabel(amplitude_label);

figure;
plot(frequency_axis_2, as_down);
title("Spectrum of filtered signal");
xlabel(frequency_label);
ylabel(amplitude_label);

figure;
plot(frequency_axis_3, as__up);
title("Spectrum of interpolated signal");
xlabel(frequency_label);
ylabel(amplitude_label);

figure;
plot(h_up);
title("Impulse responce of filter");

figure;
plot((w1 / max(w1)) * fsample3, abs(h1));
title("Frequency responce of filter");
xlabel(frequency_label);
ylabel(amplitude_label);

figure;
plot(t3, y_filtered_up);
title("Filtered signal");
xlabel(time_label);
ylabel(amplitude_label);

figure;
plot(frequency_axis_3, as_up);
title("Spectrum of filtered signal");
xlabel(frequency_label);
ylabel(amplitude_label);
