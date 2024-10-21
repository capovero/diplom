import React from 'react';
import { Formik, Form } from "formik";
import { Input, ModalContainer, ModalContent, Title, Button, LoginButton } from './modalWindow.styled';

const ModalWindow = () => {
    return (
        <ModalContainer>
            <ModalContent>
                <Title>Sign Up</Title>
                <Formik
                    initialValues={{ username: '', email: '', password: '' }}
                    onSubmit={(values, actions) => {
                        console.log(values);
                        actions.setSubmitting(false);
                    }}>
                    <Form>
                        <Input type="text" name="username" placeholder="Username" />
                        <Input type="email" name="email" placeholder="Email" />
                        <Input type="password" name="password" placeholder="Password" />
                        <Button type="submit">Sign up</Button>
                        <LoginButton type="button">Sign In</LoginButton>
                    </Form>
                </Formik>
            </ModalContent>
        </ModalContainer>
    );
}

export default ModalWindow;
