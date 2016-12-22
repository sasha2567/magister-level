% Векторная авторегрессия порядка 2
%
% Y -- матрица, в которой i-я строка -- временной ряд.
% Типа:
%   / 0.16 0.21 0.19 0.25 ... \
%   \ 0.11 0.09 0.14 0.11 ... /
function [EstSpec1, FY] = vectorAutoregressionPrediction(t_model, Y)
modelSize = size(Y);
count = round(modelSize(1));
n = modelSize(2);
predictSize = round(n / 50);

dt = logical(eye(count));
VAR2diag = vgxset('ARsolve',repmat({dt},count,1),'asolve',logical(ones(1,count)));
EstSpec1 = vgxvarx(VAR2diag,Y(:,1:(n - predictSize))');
FY = vgxpred(EstSpec1,predictSize,[],Y(:,1:(n - predictSize))');
%вывод информации о модели
vgxdisp(EstSpec1)

%Надо сделать проверку на адекватность

figure
plot(t_model, Y(1,:))
hold on
plot(t_model((n - predictSize + 1):n), FY(:,1)', 'r')

figure
plot(t_model, Y(2,:))
hold on
plot(t_model((n - predictSize + 1):n), FY(:,2)', 'r')

end

