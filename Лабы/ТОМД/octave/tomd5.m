clc;
clear all;
close all;

names(1, :) = 'TA0.WAV';
%names(2, :) = 'TA1.WAV';

for i = 1 : length(names(:, 1))
    [y, fs] = audioread(names(i, :));
    k1 = 4900;
    k2 = 1000;
    k = k1 / k2;
    r = 5;
    y_long = y(1:k1, 1);
    y_short = y(1:k2, 1);
    f_long = 0 : fs / length(y_long) : fs / 2 - fs / length(y_long);
    f_short = 0 : fs / length(y_short) : fs / 2 - fs / length(y_short);
    s_long = abs(fft(y_long));
    s_short = abs(fft(y_short));
    ls_long = log(s_long);
    ls_short = log(s_short);
    t = 0 : 1 / fs : length(y) / fs - 1 / fs;
    f = 0 : fs / length(y) : fs / 2 - fs / length(y);
    m_f_n = (fft(y))';
    ms = ones(1, length(m_f_n));
    m_f_n = m_f_n(1 : length(m_f_n) / 2);
    for i = 1 : r
        m_f = m_f_n(mod(1 : length(m_f_n), i) == 0);
        ms = ms(1 : length(m_f)) .* (abs(abs(m_f)) .^ 2);
    end;
    y1 = y';
    s_y = fft(y1);
    log_as_y = log(abs(s_y));
    c_y = ifft(log_as_y);
    w = 100;
    c_w = c_y;
    c_w(w : (length(c_y) - w)) = 0;
    s_c_y = fft(c_y);
    s_c_w = fft(c_w);

    figure;
    plot(f_long, s_long(1 : length(s_long) / 2));
    grid on;
    title('Narrow band frequency responce');
    xlabel('F, Hz');
    ylabel('Amplitude');

    figure;
    plot(f_short, s_short(1 : length(s_short) / 2));
    grid on;
    title('Wide band frequency responce');
    xlabel('F, Hz');
    ylabel('Amplitude');

    figure;
    plot(f_long, ls_long(1 : length(ls_long) / 2));
    grid on;
    title('Narrow band log frequency responce');
    xlabel('F, Hz');
    ylabel('Amplitude');

    figure;
    plot(f_short, ls_short(1 : length(ls_short) / 2));
    grid on;
    title('Wide band log frequency responce');
    xlabel('F, Hz');
    ylabel('Amplitude');
    
    figure
    plot(f(1 : length(ms)), ms);
    grid on;
    title('Production of decimated spectrums');
    xlabel('F, Hz');
    ylabel('Amplitude');

    figure;
    specgram(y', 410, fs, 20, 0);
    title('Spectrogram');

    figure;
    plot(t, y1);
    grid on;
    title('Signal');
    xlabel('T, Sec');
    ylabel('Amplitude');

    figure;
    plot(f, abs(s_y)(1: length(s_y) / 2));
    grid on;
    title('Frequency response');
    xlabel('F, Hz');
    ylabel('Amplitude');

    figure;
    plot(f, log_as_y(1: length(log_as_y) / 2));
    grid on;
    title('Log frequency response');
    xlabel('F, Hz');
    ylabel('Amplitude');

    figure;
    plot(t(1 : length(c_y) / 2), abs(c_y)(1 : length(c_y) / 2));
    grid on;
    title('Cepstrum');
    xlabel('T, Sec');
    ylabel('Amplitude');

    figure;
    plot(f, log_as_y(1 : length(f)), 'b', f, s_c_w(1 : length(f)), 'r');
    grid on;
    title('Log frequency response');
    xlabel('F, Hz');
    ylabel('Amplitude');

end;