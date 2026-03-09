#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <chrono>
#include <iomanip>

using namespace std;
using namespace std::chrono;

string generateRandomString(int length) {
    string str;
    str.reserve(length);
    for (int i = 0; i < length; ++i) {
        str += (char)('a' + rand() % 26);
    }
    return str;
}

int lev_recursive(const string& s1, int i, const string& s2, int j) {
    if (i == 0) return j;
    if (j == 0) return i;
    int cost = (s1[i - 1] == s2[j - 1]) ? 0 : 1;
    return min({
        lev_recursive(s1, i - 1, s2, j) + 1,
        lev_recursive(s1, i, s2, j - 1) + 1,
        lev_recursive(s1, i - 1, s2, j - 1) + cost
    });
}

int lev_dp(const string& s1, const string& s2) {
    int m = s1.length(), n = s2.length();
    vector<vector<int>> dp(m + 1, vector<int>(n + 1));
    for (int i = 0; i <= m; i++) dp[i][0] = i;
    for (int j = 0; j <= n; j++) dp[0][j] = j;
    for (int i = 1; i <= m; i++) {
        for (int j = 1; j <= n; j++) {
            int cost = (s1[i - 1] == s2[j - 1]) ? 0 : 1;
            dp[i][j] = min({dp[i - 1][j] + 1, dp[i][j - 1] + 1, dp[i - 1][j - 1] + cost});
        }
    }
    return dp[m][n];
}

int lcs_recursive(const string& s1, int i, const string& s2, int j) {
    if (i == 0 || j == 0) return 0;
    if (s1[i - 1] == s2[j - 1]) {
        return 1 + lcs_recursive(s1, i - 1, s2, j - 1);
    }
    return max(lcs_recursive(s1, i - 1, s2, j), lcs_recursive(s1, i, s2, j - 1));
}

int lcs_dp(const string& s1, const string& s2) {
    int m = s1.length(), n = s2.length();
    vector<vector<int>> dp(m + 1, vector<int>(n + 1));
    for (int i = 1; i <= m; i++) {
        for (int j = 1; j <= n; j++) {
            if (s1[i - 1] == s2[j - 1]) {
                dp[i][j] = dp[i - 1][j - 1] + 1;
            } else {
                dp[i][j] = max(dp[i - 1][j], dp[i][j - 1]);
            }
        }
    }
    return dp[m][n];
}

int main() {
    srand(42);

    string S1 = generateRandomString(300);
    string S2 = generateRandomString(200);

    cout << "Дистанция Левенштейна" << endl;

    auto start = high_resolution_clock::now();
    int full_dist = lev_dp(S1, S2);
    auto end = high_resolution_clock::now();
    cout << "Дистанция Левенштейна S1 и S2 (ДП): " << full_dist << endl;
    cout << "Время (ДП): " << duration_cast<microseconds>(end - start).count() << " мкс\n" << endl;

    string lev_s1 = "гора";
    string lev_s2 = "вор";
    cout << "Сравнение " << lev_s1 << " и " << lev_s2 << endl;

    start = high_resolution_clock::now();
    int lev_rec_res = lev_recursive(lev_s1, lev_s1.length(), lev_s2, lev_s2.length());
    end = high_resolution_clock::now();
    auto lev_t_rec = duration_cast<microseconds>(end - start).count();

    start = high_resolution_clock::now();
    int lev_dp_res = lev_dp(lev_s1, lev_s2);
    end = high_resolution_clock::now();
    auto lev_t_dp = duration_cast<microseconds>(end - start).count();

    cout << "Дистанция Левенштейна (Рекурсия): " << lev_rec_res << ", Время: " << lev_t_rec << " мкс" << endl;
    cout << "Дистанция Левенштейна (ДП): " << lev_dp_res << ", Время: " << lev_t_dp << " мкс\n" << endl;

    cout << "Сравнение времени на случайных подстроках:" << endl;
    cout << setw(5) << "N" << setw(15) << "Рекурсия(мкс)" << setw(15) << "ДП(мкс)" << endl;

    for (int n = 1; n <= 12; ++n) {
        string s1_sub = S1.substr(0, n);
        string s2_sub = S2.substr(0, n);

        start = high_resolution_clock::now();
        lev_recursive(s1_sub, n, s2_sub, n);
        end = high_resolution_clock::now();
        auto t_rec = duration_cast<microseconds>(end - start).count();

        start = high_resolution_clock::now();
        lev_dp(s1_sub, s2_sub);
        end = high_resolution_clock::now();
        auto t_dp = duration_cast<microseconds>(end - start).count();

        cout << setw(5) << n << setw(15) << t_rec << setw(15) << t_dp << endl;
    }

    cout << "\nНаибольшая общая подпоследовательность" << endl;
    string lcs_s1 = "ABHCSUV";
    string lcs_s2 = "KIBOSV";
    cout << "Строки варианта 9: " << lcs_s1 << " и " << lcs_s2 << endl;

    start = high_resolution_clock::now();
    int lcs_rec_res = lcs_recursive(lcs_s1, lcs_s1.length(), lcs_s2, lcs_s2.length());
    end = high_resolution_clock::now();
    auto lcs_t_rec = duration_cast<microseconds>(end - start).count();

    start = high_resolution_clock::now();
    int lcs_dp_res = lcs_dp(lcs_s1, lcs_s2);
    end = high_resolution_clock::now();
    auto lcs_t_dp = duration_cast<microseconds>(end - start).count();

    cout << "Длина LCS (Рекурсия): " << lcs_rec_res << ", Время: " << lcs_t_rec << " мкс" << endl;
    cout << "Длина LCS (ДП): " << lcs_dp_res << ", Время: " << lcs_t_dp << " мкс" << endl;

    cout << "\nСравнение времени на случайных подстроках:" << endl;
    cout << setw(5) << "N" << setw(15) << "Рекурсия(мкс)" << setw(15) << "ДП(мкс)" << endl;

    for (int n = 1; n <= 16; ++n) {
        string s1_sub = S1.substr(0, n);
        string s2_sub = S2.substr(0, n);

        start = high_resolution_clock::now();
        lcs_recursive(s1_sub, n, s2_sub, n);
        end = high_resolution_clock::now();
        auto t_rec = duration_cast<microseconds>(end - start).count();

        start = high_resolution_clock::now();
        lcs_dp(s1_sub, s2_sub);
        end = high_resolution_clock::now();
        auto t_dp = duration_cast<microseconds>(end - start).count();

        cout << setw(5) << n << setw(15) << t_rec << setw(15) << t_dp << endl;
    }

    return 0;
}
