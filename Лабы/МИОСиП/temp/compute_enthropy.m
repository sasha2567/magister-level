function [h, s, q, i] = compute_enthropy(x, n)
    [n1, ~] = hist(x, n);
    figure;
    hist(x, n);
    P1 = n1 / length(x);
    h = - (P1 * (log2(P1)'));
    s = std(x);
    q = (max(x) - min(x)) / n;
    i = (1 / 2) * log2(1 + (12 * s) / (q ^ 2));
end

