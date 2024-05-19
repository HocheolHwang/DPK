import logo from "./logo.svg";
import "./App.css";
import NavBar from "./components/NavBar";
import MainPage from "./main";
import { Route, Routes } from "react-router-dom";
import { Box } from "@mui/material";
function App() {
    return (
        <>
            <Box position={"fixed"} width={"100vw"}>
                <NavBar></NavBar>
            </Box>
            <Routes>
                <Route path="" element={<MainPage></MainPage>}></Route>
            </Routes>
        </>
    );
}

export default App;
