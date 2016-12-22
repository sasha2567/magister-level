% y -- это ряд
function [y_pred, residuals] = arimaPrediction(p, d, q, y)
model = arima(p, d, q);
modelSize = size(y);
n = modelSize(2);
predictSize = round(n / 50);

est = estimate(model, y(1:(n - predictSize))');
y_pred = simulate(est, predictSize, 'NumPaths', 1000, 'Y0', y(1:(n - predictSize))');
lower = prctile(y_pred, 2.5, 2);
upper = prctile(y_pred, 97.5, 2);
mn = mean(y_pred,2);
figure
plot(y)
hold on
plot((n - predictSize + 1):n, mn, 'g')
plot((n - predictSize + 1):n, lower, 'r')
plot((n - predictSize + 1):n, upper, 'r')

residuals = y((n - predictSize + 1):n) - mn';
end

