import { Box, Button, Container, Stack } from "@mui/material";
import MainPageBackGround from "../assets/images/main_background.gif";
import LogoImg from "../assets/images/game_logo.png";
import TeamLogo from "../assets/images/team_logo.png";
import CharactersImg from "../assets/images/characters.png";
import YouTube from "react-youtube";
import { useState } from "react";
import VideoModal from "./video-modal";
const MainPage = () => {
    const opts = {
        height: "390",
        width: "840",
        playerVars: {
            autoplay: 1,
        },
    };

    const [isOpenModal, SetIsOpenModal] = useState(false);
    const openHandler = () => {
        SetIsOpenModal(true);
    };
    const closeHandler = () => {
        SetIsOpenModal(false);
    };
    return (
        <Box>
            <Box
                width={"100%"}
                sx={{
                    width: "100%", // 너비 설정
                    height: "100vh", // 높이 설정
                    backgroundImage: `url(${MainPageBackGround})`, // 배경 이미지 URL 설정
                    backgroundSize: "cover",
                    backgroundPosition: "center", // 이미지 위치 설정
                }}
            >
                <VideoModal isOpen={isOpenModal} closeHandler={closeHandler}></VideoModal>
                <Stack direction={"column"} alignItems={"center"} justifyContent={"center"}>
                    <Box marginTop={"5vh"} bgcolor={"rgba(255,255,255,0.95)"} height={"95vh"}>
                        <Stack justifyContent={"center"} alignItems={"center"}>
                            <Box marginTop={"7vh"}>
                                <img src={LogoImg}></img>
                            </Box>
                            <Box marginTop={"2vh"} marginBottom={"4vh"} color={"#FFD257"} sx={{ fontSize: "500%" }}>
                                던전 액션 RPG
                            </Box>
                            <div onClick={openHandler} style={{ cursor: "pointer" }}>
                                <Box className="main-btn" color={"white"} bgcolor={"#FFD257"} boxShadow={0} sx={{}}>
                                    <Stack
                                        paddingY={"2%"}
                                        minWidth={"36vw"}
                                        direction={"column"}
                                        alignItems={"center"}
                                        justifyContent={"center"}
                                        sx={{ fontSize: "400%" }}
                                    >
                                        게임 소개영상
                                    </Stack>
                                </Box>
                            </div>
                            <Box marginY={"1rem"}>
                                <a href="https://k10e207.p.ssafy.io/file/DungeonProcessingKnight.zip" download={"DungeonProcessingKnight.zip"}>
                                    <Box className="main-btn" color={"white"} bgcolor={"#FFD257"} boxShadow={0}>
                                        <Stack
                                            paddingY={"2%"}
                                            minWidth={"36vw"}
                                            direction={"column"}
                                            alignItems={"center"}
                                            justifyContent={"center"}
                                            sx={{ fontSize: "400%" }}
                                        >
                                            게임 다운로드
                                        </Stack>
                                    </Box>
                                </a>
                            </Box>
                            <Box height={"32vh"} position={"fixed"} bottom={"0px"}>
                                <img height={"100%"} src={CharactersImg}></img>
                            </Box>
                        </Stack>
                    </Box>
                </Stack>
            </Box>
        </Box>
    );
};

export default MainPage;
