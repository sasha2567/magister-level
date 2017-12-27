clc;
clear;
close all;

[x fs] = audioread('TA7.WAV');

nonVocalized = x(1:1000);
vocalized = x(4900:6300);

t1 = 0 : 1/fs : length(nonVocalized)/fs - 1/fs;
t2 = 0 : 1/fs : length(vocalized)/fs - 1/fs;

wideBandSpectrumNonvocalized = abs(fft(nonVocalized, 64));
narrowBandSpectrumNonvocalized = abs(fft(nonVocalized, 1024));

wideBandSpectrumVocalized = abs(fft(vocalized, 64));
narrowBandSpectrumVocalized = abs(fft(vocalized, 1024));

f1 = 0 : fs/length(wideBandSpectrumNonvocalized) : fs - fs/length(wideBandSpectrumNonvocalized);
f2 = 0 : fs/length(narrowBandSpectrumNonvocalized) : fs - fs/length(narrowBandSpectrumNonvocalized);

f3 = 0 : fs/length(wideBandSpectrumVocalized) : fs - fs/length(wideBandSpectrumVocalized);
f4 = 0 : fs/length(narrowBandSpectrumVocalized) : fs - fs/length(narrowBandSpectrumVocalized);

pointA = vocalized;

pointB = fft(pointA);

pointC = log(abs(pointB));

f5 = 0 : fs/length(pointB) : fs - fs/length(pointB);

pointD = ifft(pointC);  
t3 = 0 : 1/fs : length(pointD)/fs - 1/fs;

pointE = pointD;
pointE(90:length(pointE) - 90) = 0;
pointF = fft(pointE);

s = narrowBandSpectrumVocalized(1 : length(narrowBandSpectrumVocalized)/2)';
s2 = ones(1, length(narrowBandSpectrumVocalized));

spectrumPartsCount = 2;
for i = 1 : spectrumPartsCount
    s1 = s(mod(1 : length(s), i) == 0);
    s2 = s2(1:length(s1)).*(s1.^2);
end

figure;
plot(x);
figure;
subplot(2, 1, 1);
plot(t1, nonVocalized);
subplot(2, 1, 2);
plot(t2, vocalized);

figure;
subplot(2, 1, 1);
plot(f1(1 : length(wideBandSpectrumNonvocalized)/2), wideBandSpectrumNonvocalized(1 : length(wideBandSpectrumNonvocalized)/2));
subplot(2, 1, 2);
plot(f2(1 : length(narrowBandSpectrumNonvocalized)/2), narrowBandSpectrumNonvocalized(1 : length(narrowBandSpectrumNonvocalized)/2));
title('Spectre nonvocalized');
xlabel('Freq, Hz');

figure;
subplot(2, 1, 1);
plot(f3(1 : length(wideBandSpectrumVocalized)/2), wideBandSpectrumVocalized(1 : length(wideBandSpectrumVocalized)/2));
subplot(2, 1, 2);
plot(f4(1 : length(narrowBandSpectrumVocalized)/2), narrowBandSpectrumVocalized(1 : length(narrowBandSpectrumVocalized)/2));
title('Spectre vocalized');
xlabel('Freq, Hz');

figure;
plot(f5(1 : length(pointC)/2), pointC(1 : length(pointC)/2))

figure;
plot(t3, abs(pointD));

figure;
plot(t3, abs(pointE));

figure(5);
plot(f5(1 : length(pointC)/2), pointF(1 : length(pointC)/2), 'r')

figure;
ff = 0 : fs/2/spectrumPartsCount/length(s2) : fs/2/spectrumPartsCount - fs/2/spectrumPartsCount/length(s2);
plot(s2);
