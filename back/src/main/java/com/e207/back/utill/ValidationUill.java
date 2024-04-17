package com.e207.back.utill;

import java.util.regex.Pattern;

public class ValidationUill {
    private static final Pattern usernamePattern = Pattern.compile("^[A-Za-z0-9]{4,}$");
    private static final Pattern passwordPattern = Pattern.compile("^[A-Za-z0-9!@#$%^&*()_+=\\-\\[\\]{};':\"\\\\|,.<>/?]+$");

    private static final Pattern nicknamePattern = Pattern.compile("^[가-힣A-Za-z0-9]+$");

    public static boolean isValidUsername(String username) {
        return usernamePattern.matcher(username).matches();
    }

    public static boolean isValidPassword(String password) {
        return passwordPattern.matcher(password).matches();
    }

    public static boolean isValidNickname(String nickname) {
        return nicknamePattern.matcher(nickname).matches();
    }
}
