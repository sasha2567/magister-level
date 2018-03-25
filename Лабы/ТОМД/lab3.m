close all;
clear all;
 
order = 6;
cutoff = 0.4;
 
file = 'Illiac_Bay.jpg';
 
image = imread(file, 'jpg');
 
grey_image = rgb2gray(image);
 
figure;
imshow(grey_image);
title('Source greyed image');
 
image_with_gaussian_noise = imnoise(grey_image, 'gaussian');
 
figure;
imshow(image_with_gaussian_noise);
title('Image with gaussian noise');
 
[f1, f2] = freqspace(order);
[x, y] = meshgrid(f1, f2);
Hd = zeros(size(x));
d = find(sqrt(x .* x + y .* y) < cutoff);
Hd(d) = ones(size(d));
figure;
mesh(x, y, Hd);
title('Wanted frequency response of filter');
h = fwind1(Hd, hamming(order));
Hdr = freqz2(h, order, order);
figure;
mesh(x, y, abs(Hdr));
title('Real frequency response of filter');
 
filtered_from_gaussian_noise = uint8(filter2(h, image_with_gaussian_noise));
 
figure;
imshow(filtered_from_gaussian_noise);
title('Filtered from gaussian noise by generated filter');
 
image_with_salt_and_pepper_noise = imnoise(grey_image, 'salt & pepper');
figure;
imshow(image_with_salt_and_pepper_noise);
title('Image with "salt and pepper" noise');
 
filtered_by_generated_filter = uint8(filter2(h, image_with_salt_and_pepper_noise));
 
figure;
imshow(filtered_by_generated_filter);
title('Filtered from "salt and pepper" noise by generated filter');
 
filtered_by_median_filter = medfilt2(image_with_salt_and_pepper_noise);
 
figure;
imshow(filtered_by_median_filter);
title('Filtered from "salt and pepper" noise by median filter');
