package com.e207.back.dto;


import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;

@Getter
@Setter
@AllArgsConstructor
public class ResponseDto {

	private String message;

	public ResponseDto() {
		this.message = "success";
	}

	public static ResponseEntity<ResponseDto> success() {
		ResponseDto responseBody = new ResponseDto();
		return ResponseEntity.status(HttpStatus.OK).body(responseBody);
	}

	public static ResponseEntity<ResponseDto> databaseError() {
		ResponseDto responseBody = new ResponseDto(ResponseMessage.DATABASE_ERROR);
		return ResponseEntity.status(HttpStatus.NOT_FOUND).body(responseBody);
	}

	public static ResponseEntity<ResponseDto> validationFail() {
		ResponseDto responseBody = new ResponseDto(ResponseMessage.VALIDATION_FAIL);
		return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(responseBody);
	}

	public static ResponseEntity<ResponseDto> jwtTokenFail() {
		ResponseDto responseBody = new ResponseDto(ResponseMessage.VALIDATION_FAIL);
		return ResponseEntity.status(HttpStatus.FORBIDDEN).body(responseBody);
	}
}
