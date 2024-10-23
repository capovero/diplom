import styled from "@emotion/styled";
import {Field} from "formik";

export const ModalContainer = styled.div`
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    background-color: rgba(40, 167, 69, 0.4); /* Темно-синий фон */
`;

export const ModalContent = styled.div`
    background-color: #ffffff; /* Белый фон для формы */
    padding: 40px;
    border-radius: 16px; /* Закругленные углы */
    box-shadow: 0px 4px 15px rgba(0, 0, 0, 0.1);
    width: 320px; /* Ширина модалки */
    text-align: center; /* Центрирование контента */
`;

export const Title = styled.h1`
    color: #ffcc00; /* Золотистый цвет заголовка */
    margin-bottom: 20px;
    font-size: 28px;
`;

export const Button = styled.button`
    background-color: #28a745; /* Зеленый цвет для кнопки */
    color: #ffffff;
    padding: 10px;
    border: none;
    border-radius: 8px;
    cursor: pointer;
    width: 100%;
    margin-top: 15px;
    font-size: 16px;
    transition: background-color 0.3s ease;

    &:hover {
        background-color: #218838; /* Темнее при наведении */
    }
`;

export const LoginButton = styled.button`
    background: none;
    color: #28a745; /* Зеленый цвет */
    margin-top: 10px;
    font-size: 14px;
    cursor: pointer;
    border: none;
    text-decoration: underline;
`;

export const Input = styled(Field)`
    padding: 10px;
    border: 2px solid #f2f2f2;
    border-radius: 8px;
    background-color: #ffffff;
    color: #003366;
    font-size: 16px;
    width: 100%;
    margin-bottom: 15px;
    outline: none;
    transition: border-color 0.3s ease, box-shadow 0.3s ease;

    &:focus {
        border-color: #28a745; /* Зеленый для акцента */
        box-shadow: 0 0 8px rgba(40, 167, 69, 0.4); /* Мягкая зеленая тень */
    }

    &::placeholder {
        color: #999999; /* Серый для плейсхолдера */
    }
`;
