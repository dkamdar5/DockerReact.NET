import React, { FunctionComponent } from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';

type LayoutProps = {
}

export const Layout: FunctionComponent<LayoutProps> = (props) => {
  return (
    <div>
      <NavMenu />
      <Container>
        {props.children}
      </Container>
    </div>
  );
}
