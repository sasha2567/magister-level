% �������� ����
% ������ ��������� �� �������� � ����� generateProcess.m
% �, ��������, ����
clear all
close all
clc

[y_sum, t_model] = generateProcess;
y_sum2 = generateProcess;

modelSize = size(t_model);
n = modelSize(2);

%���������� ��������������� �������� � ��������������������� ����������
m1 = mean(y_sum)
sigma1 = var(y_sum)

m2 = mean(y_sum2)
sigma2 = var(y_sum2)

%���������� ��� � ����
acf = crosscorr(y_sum, y_sum, (n - 1));
figure
plot(acf)

pacf = parcorr(y_sum);
%parcorr(y_sum, (n - 1));
figure
plot(pacf)

%������ ��������� d (���������� i)
i = 0;
y_integr = y_sum;
while not(adftest(y_integr))%���� ����-������
    y_integr = diff(y_integr);
    i = i + 1;
end

%���������� ��� � ���� ���������������� ��������

acfi = crosscorr(y_integr, y_integr, (n - i - 1));
figure
plot(acfi)

pacfi = parcorr(y_integr);
figure
plot(pacfi)

%�� �������� ����������� ��� � ���� ���� ����� ��������� p � q
%��� �� ��������� �������� � ������ �. 10 ����� ���������
%� ���� ���������� p == q != 0, � � ������������ 2 ��������

%p == q == 1

p = 1;
q = 1;

[y_pred, residuals] = arimaPrediction(p, i, q, y_sum);

%�������� �� ������������ -- �������������� ��������
p1q1cov = cov(residuals)

%� p == q == 2

p=2;
q=2;

[y_pred, residuals] = arimaPrediction(p, i, q, y_sum);

%�������� �� ������������ -- �������������� ��������
p2q2cov = cov(residuals)

% Y -- �������, � ������� i-� ������ -- ��������� ���.
% ����:
%   / 0.16 0.21 0.19 0.25 ... \
%   \ 0.11 0.09 0.14 0.11 ... /
Y = [y_sum; y_sum2];

%��������� �������������
vectorAutoregressionPrediction(t_model, Y);

%���������� ������� 2-� ����� ������� Y
figure
plot(t_model,Y(1,:),t_model,Y(2,:))

%���� �����-���������� (�������� �� ������������)
cointegration = egcitest(Y')

