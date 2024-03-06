import React, { useState, useMemo, useEffect } from "react";
import styled from "styled-components";
import { restClient } from "polygon.io";

const MainBlock = styled.div`
    display: block;
    width: 100vw;
    height: 100vh;
    position: relative;
`;

const Header = styled.h1`
    display: block;
    font-size: 36px;
    font-family: Arial, Helvetica, sans-serif;
    font-weight: 800;
    font-style: italic;
    text-align: center;
`;

const UsdPair = styled.div`
    width: fit-content;
    height: fit-content;
    display: flex;
    flex-direction: column;
    flex-wrap: wrap;
    position: absolute;
    top: 15%;
    left: 15vw;
`;

const CryptoCurrency = styled.div`
    width: fit-content;
    height: fit-content;
    display: flex;
    flex-direction: column;
    flex-wrap: wrap;
    margin-left: 60%;
    position: absolute;
    top: 15%;
    left: 15vw;
`;

const StyledH2 = styled.h2`
    font-size: 16px;
    font-style: italic;
    font-weight: bolder;
    padding: 2px;
`;

const StyledSpan = styled.span`
    font-size: 16px;
    font-style: italic;
    padding: 2px;
`;

const InputForm = styled.div`
    width: 384px;
    height: 42px;
    display: flex;
    flex-direction: row;
    flex-wrap: nowrap;
    margin: 0 auto;
    margin-top: 64px;
`;

const StyledKeyInput = styled.input`
    width: 80%;
    height: auto;
    background-color: #ffffff;
    border: 1px solid #f16969;
    border-radius: 6px;
`;

const StyledSendButton = styled.button`
    width: 16%;
    margin-left: 2%;
    height: auto;
    text-align: center;
    background-color: #2284d4;
    color: white;
    border: 2px solid #e8e9eb;
    border-radius: 6px;
`;

export const MainPage = () => {

    const [usdData, setUsdData] = useState();
    const [cryptoData, setCryptoData] = useState();
    const [apiKey, setApiKey] = useState();

    const rest = restClient(apiKey);

    const handleApiKeyInput = (event) => {
        setApiKey(event.target.value);
    };

    const formattedCurrentDate = useMemo(() => {
        const newDate = new Date();
        const year = newDate.getFullYear();
        const month = newDate.getMonth() + 1;
        const d = newDate.getDate();

        return `${year}-${month.toString().padStart(2, '0')}-${d.toString().padStart(2, '0')}`;
    }, []);

    const getUsdData = async () => {
        await rest.stocks.aggregates("C:EURUSD", 1, "day", "2024-02-01", `${formattedCurrentDate}`).then((data) => {
            setUsdData(data.results[0]);
        }).catch(e => {
            console.error('An error happened:', e);
        });
    };

    const getCryptoData = async () => {
        await rest.stocks.aggregates("X:BTCUSD", 1, "day", "2024-02-01", `${formattedCurrentDate}`).then((data) => {
            setCryptoData(data.results[0]);
        }).catch(e => {
            console.error('An error happened:', e);
        });
    };

    const handleSendClick = () => {
        getCryptoData();
        getUsdData();
    };

    useEffect(() => {
        if (usdData !== undefined && cryptoData !== undefined) {
            setApiKey("");
        }
    }, [usdData, cryptoData]);

    return (
        <MainBlock className="main-page">
            <Header>
                Demo App With Poligon IO data.
            </Header>
            {usdData === undefined && cryptoData === undefined && (
                <InputForm>
                    <StyledKeyInput
                        type="text"
                        value={apiKey}
                        placeholder="Add your Poligon.io API key"
                        onChange={handleApiKeyInput}
                    />
                    <StyledSendButton onClick={handleSendClick}>Send</StyledSendButton>
                </InputForm>
            )}
            {(apiKey !== undefined && usdData !== undefined) && (
                <UsdPair>
                    <StyledH2>EUR/USD</StyledH2>
                    <StyledSpan>C: {usdData.c}</StyledSpan>
                    <StyledSpan>Close: {usdData.close}</StyledSpan>
                    <StyledSpan>H: {usdData.h}</StyledSpan>
                    <StyledSpan>High: {usdData.high}</StyledSpan>
                    <StyledSpan>L: {usdData.l}</StyledSpan>
                    <StyledSpan>Low: {usdData.low}</StyledSpan>
                    <StyledSpan>O: {usdData.o}</StyledSpan>
                    <StyledSpan>Open: {usdData.open}</StyledSpan>
                </UsdPair>
            )}
            {(apiKey !== undefined && cryptoData !== undefined) && (
                <CryptoCurrency>
                    <StyledH2>BTC/USD</StyledH2>
                    <StyledSpan>C: {cryptoData.c}</StyledSpan>
                    <StyledSpan>Close: {cryptoData.close}</StyledSpan>
                    <StyledSpan>H: {cryptoData.h}</StyledSpan>
                    <StyledSpan>High: {cryptoData.high}</StyledSpan>
                    <StyledSpan>L: {cryptoData.l}</StyledSpan>
                    <StyledSpan>Low: {cryptoData.low}</StyledSpan>
                    <StyledSpan>O: {cryptoData.o}</StyledSpan>
                    <StyledSpan>Open: {cryptoData.open}</StyledSpan>
                </CryptoCurrency>
            )}
        </MainBlock>
    );
};
