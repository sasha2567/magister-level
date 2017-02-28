clc
clear
%% ����������
keySet = {' ', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', ...
    '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', ...
    '�', '�', '�', '�', '�', '�', '�', '�', '�', '�'};
valueSet = [0.175, 0.090, 0.072, 0.062, 0.062, 0.053, 0.053, 0.045, ...
    0.040, 0.038, 0.035, 0.028, 0.026, 0.025, 0.023, 0.021, 0.018, ...
    0.016, 0.016, 0.014, 0.014, 0.013, 0.012, 0.010, 0.009, 0.007, ...
    0.006, 0.006, 0.004, 0.003, 0.003, 0.002, 0.014];
mapABC = containers.Map(keySet,valueSet);
%% �������� ������
message = '��������� ��������� ��������  21 06 1994';
message_txt = '��������� ��������� ��������   ';
%% 1 - �������������� ������� ��������
% ������ ���������� ���������� � ��������
Ichar1 = log2(32); % ���-�� ���������� �� ���� �����
Inumb1 = log2(10); % ���-�� ���������� �� ���� �����
N = length(message); % ����� ���������
Nchar = length(message) - 8; % ���-�� ����� � ���������
Nnumb = 8; % ���-�� �����
Ichar = Nchar * Ichar1;
Inumb = Nnumb * Inumb1;
Itotal = Ichar + Inumb;
% ������ �������� ���������
Htotal = (Ichar + Inumb)/(Nchar + Nnumb);
disp(['�������������� ������� ��������'])
disp(['���-�� ����: ' num2str(Nchar)])
disp(['���-�� ���������� (�����): ' num2str(Ichar)])
disp(['���-�� ���������� (�����): ' num2str(Inumb)])
disp(['���-�� ���������� (�����): ' num2str(Itotal)])
disp(['�������� (�����): ' num2str(Htotal)])
disp([' '])
%% 2 - ���������������� ������� ��������
% �������� ��������� �����
Hchar = 0.0;
T = zeros(1, length(message_txt));
T2 = zeros(1, length(message_txt));
for i = 1:length(message_txt)
    Hchar = Hchar - mapABC(message_txt(i))*log2(mapABC(message_txt(i)));
    T(i) = mapABC(message_txt(i));
    T2(i) = mapABC(message_txt(i))*log2(mapABC(message_txt(i)));
end
Hchar2 = -sum(T2);
% ���������� ���������� ��������� �����
Ichar = Nchar*Hchar;
% ����� ���������� ����������
I = Ichar + Inumb;
% ����� �������� ���������
H = I/N;
disp(['���������������� ������� ��������'])
disp(['���-�� ���������� (��������� �����): ' num2str(Ichar)])
disp(['���-�� ���������� (�������� �����): ' num2str(Inumb)])
disp(['�������� (�����): ' num2str(H)])
disp([' '])
%% 3 - ������������ � ������������� ���������
H2 = 3.52;
H3 = 3.01;
% ���������� ����������
I2 = H2*Nchar + Inumb;
I3 = H3*Nchar + Inumb;
% �������� ���������
H2 = I2/N;
H3 = I3/N;
disp(['������������ � ������������� ���������'])
disp(['���-�� ���������� (2x): ' num2str(I2)])
disp(['�������� (2x): ' num2str(H2)])
disp(['���-�� ���������� (3x): ' num2str(I3)])
disp(['�������� (3x): ' num2str(H3)])
TT = [T' abs(T2)']