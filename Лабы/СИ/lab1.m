clc;
clear;
close all;
npusk = 2;
for  k=1 : npusk
    sim('first');
    out = simout.signals.values;
    figure(k);
    plot(tout,out)
    meanValue = mean(out)
    cmp = cumprod(out);
    name = k * 10 + 1;
    figure(name);
    stem(tout,cmp)
    sumValue = sum(out)
end;