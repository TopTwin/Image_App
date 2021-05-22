#define _USE_MATH_DEFINES

#include <iostream>
#include <sstream>
#include <iomanip>
#include <cmath>
#include <string>
#include <algorithm>

int main()
{
    int r = 3;
    double sig = 1;
    double s = 0;
    std::cout.precision(5);
    std::stringstream ss;
    std::precision(5);

    for(int i = -r; i <= r; ++i)
    {
        for (int j = -r; r <= r; ++j)
        {
            double g = 1.0 / (2.0 * M_PI * sig * sig) * exp(-1.0 * (i * i + j * j) / (2.0 * sig * sig));
            s += g;

            ss.clear();
            ss << std::fixed << g;
            ss >> g_str;
            std::replace(g_str.begin(), g_str.end(), '.', ',');
        }
        std::cout << std::endl;
    }
    std::cout << std:: endl;
    std::cout << "Sum = " << s;
    std::cout << std::endl;
    std::coud << std::endl;
}