package com.e207.back.util;

import java.util.regex.Pattern;

public class ValidationUtil {
    private static final Pattern playerIdPattern = Pattern.compile("^[A-Za-z0-9]{4,12}$");
    private static final Pattern passwordPattern = Pattern.compile("^[A-Za-z0-9!@#$%^&*()_+=\\-\\[\\]{};':\"\\\\|,.<>/?]{8,16}$");

    private static final Pattern nicknamePattern = Pattern.compile("^[가-힣A-Za-z0-9]{4,12}$");

    public static boolean isValidPlayerId(String username) {
        return playerIdPattern.matcher(username).matches();
    }

    public static boolean isValidPassword(String password) {
        return passwordPattern.matcher(password).matches();
    }

    public static boolean isValidNickname(String nickname) {
        return nicknamePattern.matcher(nickname).matches();
    }
}
