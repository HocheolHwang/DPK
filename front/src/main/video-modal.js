import { Box, Container, Stack } from "@mui/material";
import YouTube from "react-youtube";
import CloseIcon from "@mui/icons-material/Close";
import DeleteIcon from "@mui/icons-material/Delete";
import { Close, Scale } from "@mui/icons-material";

const VideoModal = ({ isOpen, closeHandler }) => {
    if (isOpen == false) return <></>;
    console.log(isOpen);
    console.log(closeHandler);
    const opts = {
        height: "648",
        width: "1152",
        playerVars: {
            autoplay: 1,
        },
    };

    return (
        <>
            <div onClick={closeHandler} style={{ position: "fixed", width: "100vw", height: "100vh", zIndex: 100, backgroundColor: "rgba(0,0,0,0.75)" }}></div>
            <Box position={"fixed"} left={"50%"} top={"50%"} sx={{ transform: `translate(-50%, -50%)` }} zIndex={200}>
                <Stack justifyContent={"center"} alignItems={"center"} marginBottom={"10vh"}>
                    <div onClick={closeHandler}>
                        <CloseIcon color="primary" fontSize="large" sx={{ zoom: 3, cursor: "pointer" }}></CloseIcon>
                    </div>
                    <YouTube videoId={"Yjxo07JUp_Q"} opts={opts} />
                </Stack>
            </Box>
        </>
    );
};

export default VideoModal;
